using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQOrdering.Api.Commands;
using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Extensions;
using RabbitMQOrdering.Api.Helpers;

namespace RabbitMQOrdering.Api.Infrastructure.RabbitMQ
{
    public class ConsumeRabbitMQHostedService : BackgroundService {

        private CommandHandler _commandHandler;
        private readonly ILogger<ConsumeRabbitMQHostedService> _logger;

        private RabbitMQConfigurations _configurations; 
        private ConnectionFactory _factory; 
        private IConnection _connection;
        private IServiceProvider _serviceProvider;
        private IModel _channel;
        private CancellationTokenSource _stoppingCts =
                                                   new CancellationTokenSource();

        public ConsumeRabbitMQHostedService (IServiceProvider serviceProvider,
                                            ILogger<ConsumeRabbitMQHostedService> logger,
                                            CommandHandler commandHandler) {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _channel = (IModel) _serviceProvider.GetService (typeof (IModel));
            _connection = (IConnection) _serviceProvider.GetService(typeof(IConnection));
            _configurations = (RabbitMQConfigurations) _serviceProvider.GetService(typeof(RabbitMQConfigurations));
            _factory = (ConnectionFactory) _serviceProvider.GetService(typeof(ConnectionFactory));
            _commandHandler = commandHandler;
        }

        protected override async Task ExecuteAsync (CancellationToken stoppingToken) 
        {

            try
            {
                while (true)
                {
                    if(_connection.IsOpen)
                    {
                        Console.WriteLine("Connected to RabbitMQ...");
                        await ListenToRabbitMQ();
                    }
                    else
                    {
                        _connection.Close();
                        _channel.Close();
                        Console.WriteLine("Not connected to RabbitMQ");
                        _connection = _factory.CreateConnection();
                        _channel = _connection.CreateModel();
                        _channel.AddChannels();
                        RabbitMQExtensions.EnsureExistRabbitMqVirtualHost(_configurations, _connection);
                    }     
                    
                    await Task.Delay(2000);
                }
            }
            catch
            {
                _stoppingCts =  new CancellationTokenSource();

                await ExecuteAsync(_stoppingCts.Token);   
            }
            
        }

        private void HandleMessage (string content) {
            // we just print this message   
            _logger.LogInformation ($"consumer received {content}");
        }

        private async Task ListenToRabbitMQ()
        {
            var consumer = new EventingBasicConsumer (_channel);
            consumer.Received += (ch, ea) => {
                // received message  
                var content = System.Text.Encoding.UTF8.GetString (ea.Body);

                // handle the received message  
                HandleMessage (content);
                _channel.BasicAck (ea.DeliveryTag, false);

                Console.WriteLine("Message received");
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            var result = await _channel.BasicConsumers();

            if(result.Item2 != null && result.Item3 != "")
            {

                _commandHandler.SetCommand(result.Item3, result.Item2);
                _commandHandler.ProcessCommands();
                await Task.FromResult<ProductQueue>(result.Item2);
            }
        } 

        private void OnConsumerConsumerCancelled (object sender, ConsumerEventArgs e) { }
        private void OnConsumerUnregistered (object sender, ConsumerEventArgs e) { }
        private void OnConsumerRegistered (object sender, ConsumerEventArgs e) { }
        private void OnConsumerShutdown (object sender, ShutdownEventArgs e) { }
        private void RabbitMQ_ConnectionShutdown (object sender, ShutdownEventArgs e) 
        {
            Console.WriteLine(e.ReplyText);
        }

        public override void Dispose () {
            _channel.Close ();
            _stoppingCts.Cancel();
            base.Dispose ();
        }
    }
}