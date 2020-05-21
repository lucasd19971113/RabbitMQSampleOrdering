using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Helpers;

namespace RabbitMQOrdering.Api.Infrastructure.RabbitMQ
{
    public class RabbitMQConsumeHelper : IRabbitMQConsumeHelper
    {
        private RabbitMQConfigurations configurations; 
        private ConnectionFactory factory; 
        private IConnection connection;
        private readonly CancellationTokenSource _stoppingCts =
                                                   new CancellationTokenSource();

        public RabbitMQConsumeHelper(
                                    RabbitMQConfigurations configurations, 
                                    ConnectionFactory factory, 
                                    IConnection connection)
        {
            this.configurations = configurations;
            this.factory = factory;
            this.connection = connection;
        }

        public Task PublishToQueueAsync(KitchenArea kitchenArea, ProductQueueDto productQueue)
        {
            if(connection.IsOpen)
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ConfirmSelect();
                    var queue = channel.QueueDeclare(queue: kitchenArea.Name+" - waiting",
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);
                    
                    string message = JsonConvert.SerializeObject(productQueue);
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                        routingKey: kitchenArea.Name+" - waiting",
                                        basicProperties: null,
                                        body: body);
                    
                    channel.WaitForConfirmsOrDie(new TimeSpan(0, 0, 1));

                    return Task.CompletedTask;
                }
            }

            return Task.FromCanceled(_stoppingCts.Token);
        }
    }

    public interface IRabbitMQConsumeHelper
    {
        Task PublishToQueueAsync(KitchenArea kitchenArea, ProductQueueDto productQueue);
    }
}