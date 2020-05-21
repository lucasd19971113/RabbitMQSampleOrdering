using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQOrdering.Queue.Api.Entities;
using RabbitMQOrdering.Queue.Api.Extensions;

namespace RabbitMQOrdering.Queue.Api.Infrastructure.RabbitMQ
{
    public class ConsumeRabbitMQService : IConsumeRabbitMQService{
        private IServiceProvider _serviceProvider;
        private IModel _channel;

        public ConsumeRabbitMQService (IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
            _channel = (IModel) _serviceProvider.GetService (typeof (IModel));
        }

        public async Task<ProductQueue> ProcessQueue (string QueueName) {
            
            int messagesRead = 0;

            var consumer = new EventingBasicConsumer (_channel);
            consumer.Received += (ch, ea) => {

                //if(messagesRead == 0)
                //{
                    // received message  
                    var content = System.Text.Encoding.UTF8.GetString (ea.Body);

                    // handle the received message  
                    HandleMessage (content);

                    _channel.BasicQos(1,1, false);
                    //_channel.BasicAck (ea.DeliveryTag, false);

                    messagesRead ++;

                    Console.WriteLine("Mensagem recebida");
                //}
                
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            //if(messagesRead == 1)
                var result = await _channel.BasicGetQueue(QueueName);

                if(result.Item2 != null)
                {
                    return await Task.FromResult<ProductQueue>(result.Item2).ConfigureAwait(true);
                }
            
            
            return null;
        }

        public Task ProcessAllQueues () {
            
            int messagesRead = 0;

            var consumer = new EventingBasicConsumer (_channel);
            consumer.Received += (ch, ea) => {

                //if(messagesRead == 0)
                //{
                    // received message  
                    var content = System.Text.Encoding.UTF8.GetString (ea.Body);

                    // handle the received message  
                    HandleMessage (content);

                    //_channel.BasicQos(1,1, false);
                    //_channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                    messagesRead ++;

                    Console.WriteLine("Mensagem recebida");
                //}
                
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            //if(messagesRead == 1)
                var result = _channel.BasicConsumers();

            return Task.CompletedTask;
        }

        private void HandleMessage (string content) {
            // we just print this message   
            
        }

        private void OnConsumerConsumerCancelled (object sender, ConsumerEventArgs e) { }
        private void OnConsumerUnregistered (object sender, ConsumerEventArgs e) { }
        private void OnConsumerRegistered (object sender, ConsumerEventArgs e) { }
        private void OnConsumerShutdown (object sender, ShutdownEventArgs e) { }
        private void RabbitMQ_ConnectionShutdown (object sender, ShutdownEventArgs e) { }

        /*public override void Dispose () {
            _channel.Close ();
            _connection.Close ();
            base.Dispose ();
        }*/
    }

    public interface IConsumeRabbitMQService
    {
        Task<ProductQueue> ProcessQueue (string QueueName);
        Task ProcessAllQueues ();
    }
}