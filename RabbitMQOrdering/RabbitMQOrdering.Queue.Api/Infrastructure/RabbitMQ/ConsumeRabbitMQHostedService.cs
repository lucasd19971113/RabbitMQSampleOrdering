using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQOrdering.Queue.Api.Extensions;
using RabbitMQOrdering.Queue.Api.Helpers;

namespace RabbitMQOrdering.Queue.Api.Infrastructure.RabbitMQ
{
    public class ConsumeRabbitMQHostedService : BackgroundService {

        private readonly ILogger<ConsumeRabbitMQHostedService> _logger;

        private RabbitMQConfigurations _configurations; 
        private ConnectionFactory _factory; 
        private IConnection _connection;
        private IServiceProvider _serviceProvider;
        private IModel _channel;
        private CancellationTokenSource _stoppingCts =
                                                   new CancellationTokenSource();

        public ConsumeRabbitMQHostedService (IServiceProvider serviceProvider,
                                            ILogger<ConsumeRabbitMQHostedService> logger) {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _channel = (IModel) _serviceProvider.GetService (typeof (IModel));
            _connection = (IConnection) _serviceProvider.GetService(typeof(IConnection));
            _configurations = (RabbitMQConfigurations) _serviceProvider.GetService(typeof(RabbitMQConfigurations));
            _factory = (ConnectionFactory) _serviceProvider.GetService(typeof(ConnectionFactory));
        }

        protected override async Task ExecuteAsync (CancellationToken stoppingToken) 
        {

            try
            {
                while (true)
                {
                    if(!_connection.IsOpen)
                    {
                        
                        _connection.Close();
                        _channel.Close();
                        Console.WriteLine("Not connected to RabbitMQ");
                        _connection = _factory.CreateConnection();
                        _channel = _connection.CreateModel();
                        _channel.AddChannels();
                        RabbitMQExtensions.EnsureExistRabbitMqVirtualHost(_configurations, _connection);
                    }
                    else
                    {
                        Console.WriteLine("Connected to RabbitMQ...");
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

        public override void Dispose () {
            _channel.Close ();
            _stoppingCts.Cancel();
            base.Dispose ();
        }
    }
}