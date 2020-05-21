using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQOrdering.Queue.Api.Entities;
using RabbitMQOrdering.Queue.Api.Helpers;

namespace RabbitMQOrdering.Queue.Api.Extensions {
    public static class RabbitMQExtensions {
        // public static IModel AddChannels(this IModel channel, string queue, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments
        public static IModel AddChannels (this IModel channel) {

            var queue = channel.QueueDeclare (queue: "Side Dish - waiting",
                durable : false,
                exclusive : false,
                autoDelete : false,
                arguments : null);

            channel.QueueDeclare (queue: "Meal - waiting",
                durable : false,
                exclusive : false,
                autoDelete : false,
                arguments : null);

            channel.QueueDeclare (queue: "Desert - waiting",
                durable : false,
                exclusive : false,
                autoDelete : false,
                arguments : null);

            channel.QueueDeclare (queue: "Beverage - waiting",
                durable : false,
                exclusive : false,
                autoDelete : false,
                arguments : null);

            channel.QueueDeclare (queue: "Side Dish - preparing",
                durable : false,
                exclusive : false,
                autoDelete : false,
                arguments : null);

            channel.QueueDeclare (queue: "Meal - preparing",
                durable : false,
                exclusive : false,
                autoDelete : false,
                arguments : null);

            channel.QueueDeclare (queue: "Desert - preparing",
                durable : false,
                exclusive : false,
                autoDelete : false,
                arguments : null);

            channel.QueueDeclare (queue: "Beverage - preparing",
                durable : false,
                exclusive : false,
                autoDelete : false,
                arguments : null);

            channel.QueueDeclare (queue: "Command Queue",
                durable : false,
                exclusive : false,
                autoDelete : false,
                arguments : null);

            return channel;
        }

        public static void EnsureExistRabbitMqVirtualHost (RabbitMQConfigurations cfg, IConnection connection) {
            if(connection.IsOpen)
            {    var credentials = new NetworkCredential () { UserName = cfg.UserName, Password = cfg.Password };
                using (var handler = new HttpClientHandler { Credentials = credentials })
                using (var client = new HttpClient (handler)) {
                    var url = $"http://{cfg.HostName}:{cfg.ManagementPort}/api/vhosts/{cfg.VirtualHost}";

                    var content = new StringContent ("", Encoding.UTF8, "application/json");
                    var result = client.PutAsync (url, content).Result;

                    if ((int) result.StatusCode >= 300)
                        throw new Exception (result.ToString ());
                }
            }
        }

        public static List<ProductQueueDto> GetAllMessages (QueueDeclareOk queue, RabbitMQConfigurations cfg, string queueName) {
            var credentials = new NetworkCredential () { UserName = cfg.UserName, Password = cfg.Password };
            using (var handler = new HttpClientHandler { Credentials = credentials })
            using (var client = new HttpClient (handler)) {
                var url = $"http://{cfg.HostName}:{cfg.ManagementPort}/api/queues/{cfg.VirtualHost}/{queueName}/get";

                var jsonContent = new { 
                    ackmode = "ack_requeue_true",
                    count = queue.MessageCount.ToString(),
                    encoding = "auto",
                    truncate = "50000",
                    vhost = cfg.VirtualHost
                };

                var content = new StringContent (JsonConvert.SerializeObject(jsonContent), Encoding.UTF8, "application/json");
                var result = client.PostAsync (url, content).Result.Content.ReadAsStringAsync().Result;

                List<QueuePayload> queuePayload = new List<QueuePayload>();
                var objectList = JsonConvert.DeserializeAnonymousType(result, queuePayload);
                var ProductQueueList = new List<ProductQueueDto>();
                if(objectList.AsEnumerable().Any())
                {
                    objectList.ForEach(pl => {
                        if(pl.payload != null)
                        {
                            var product = JsonConvert.DeserializeObject<ProductQueueDto>(pl.payload);
                            ProductQueueList.Add(product);
                        }
                        
                    });

                    return ProductQueueList;
                }

                return null;

                
            }
        }

        public static IModel BasicConsumers (this IModel channel) {

            var consumer = new EventingBasicConsumer (channel);

            channel.BasicConsume (queue: "Side Dish - waiting",
                autoAck : true,
                consumer: consumer);

            channel.BasicConsume (queue: "Meal - waiting",
                autoAck : true,
                consumer: consumer);

            channel.BasicConsume (queue: "Desert - waiting",
                autoAck : true,
                consumer: consumer);

            channel.BasicConsume (queue: "Beverage - waiting",
                autoAck : true,
                consumer: consumer);

            channel.BasicConsume (queue: "Side Dish - preparing",
                autoAck : true,
                consumer: consumer);

            channel.BasicConsume (queue: "Meal - preparing",
                autoAck : true,
                consumer: consumer);

            channel.BasicConsume (queue: "Desert - preparing",
                autoAck : true,
                consumer: consumer);

            channel.BasicConsume (queue: "Beverage - preparing",
                autoAck : true,
                consumer: consumer);

            return channel;
        }

        public static async Task<(IModel, ProductQueue)> BasicGetQueue(this IModel channel, string QueueName) {

            var consumer = new EventingBasicConsumer (channel);

            var result = channel.BasicGet (queue: QueueName,
                autoAck : true);

            
            if(result != null){

                string s = System.Text.Encoding.UTF8.GetString(result.Body, 0, result.Body.Length);

                if(s != "" && s != null)
                {
                    var objectResult = JsonConvert.DeserializeObject<ProductQueue>(s);

                    if(objectResult != null)
                        return await Task.FromResult((channel, objectResult)).ConfigureAwait(true);
                    
                    return (channel, null);
                }
                
                return (channel, null);
            }

            return (channel, null);

        }

        private static void Consumer_Received (
            object sender, BasicDeliverEventArgs e) {
            var message = Encoding.UTF8.GetString (e.Body);
            Console.WriteLine (Environment.NewLine +
                "[Nova mensagem recebida] " + message);
        }
    }
}