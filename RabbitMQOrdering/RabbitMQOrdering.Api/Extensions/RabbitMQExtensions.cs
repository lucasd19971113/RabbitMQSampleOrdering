using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Helpers;

namespace RabbitMQOrdering.Api.Extensions {
    public static class RabbitMQExtensions {
        // public static IModel AddChannels(this IModel channel, string queue, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments
        public static IModel AddChannels (this IModel channel) {

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

        public static void GetAllMessages (QueueDeclareOk queue, RabbitMQConfigurations cfg, string queueName) {
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

                
            }
        }

        public static async Task<(IModel, ProductQueue, string)> BasicConsumers (this IModel channel) {

            var consumer = new EventingBasicConsumer (channel);

            var result = channel.BasicGet (queue: "Command Queue",
                autoAck : true);

            
            if(result != null){

                string s = System.Text.Encoding.UTF8.GetString(result.Body, 0, result.Body.Length);

                var headers = result.BasicProperties.Headers;

                string command = "";


                foreach (var item in headers)
                {
                    if(item.Key.ToLower() == "command")
                    {
                        var test = item.Value;

                        
                        var commandBytes = (byte[]) result.BasicProperties.Headers["Command"];
                        command = Encoding.UTF8.GetString(commandBytes);

                        Console.WriteLine("Command to be executed {0}", command);
                        
                    }
                }

                if(s != "" && s != null)
                {
                    var objectResult = JsonConvert.DeserializeObject<ProductQueue>(s);
                    Console.WriteLine("Comando recebido");
                    if(objectResult != null && command != "")
                        return await Task.FromResult((channel, objectResult, command)).ConfigureAwait(true);
                    
                    return (channel, null, null);
                }
                
                return (channel, null, null);
            }

            return (channel, null, null);
        }
        

        private static void Consumer_Received (
            object sender, BasicDeliverEventArgs e) {
            var message = Encoding.UTF8.GetString (e.Body);
            Console.WriteLine (Environment.NewLine +
                "[Nova mensagem recebida] " + message);
        }
    }
}