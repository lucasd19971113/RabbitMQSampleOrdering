using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQOrdering.Queue.Api.Extensions;
using RabbitMQOrdering.Queue.Api.Helpers;
using RabbitMQOrdering.Queue.Api.Infrastructure.RabbitMQ;

namespace RabbitMQOrdering.Queue.Api.Controllers
{
    [Produces ("application/json")]
    [Route ("api/[controller]/[action]")]
    [ApiController]
    public class QueueController : Controller
    {

        public QueueController()
        {
            
        }

        [HttpGet]
        [Route("{kitchenAreaName}/{status}")]
        public IActionResult ShowQueueItem(
                                [FromServices]RabbitMQConfigurations configurations, 
                                [FromServices]ConnectionFactory factory, 
                                [FromServices]IConnection connection,
                                [FromServices]IConsumeRabbitMQService consumer,
                                string kitchenAreaName,
                                string status)
        {
            try
            {
                using (var channel = connection.CreateModel())
                {
                    var queue = channel.QueueDeclare (queue: kitchenAreaName+" - "+status,
                        durable : false,
                        exclusive : false,
                        autoDelete : false,
                        arguments : null);

                    var result = RabbitMQExtensions.GetAllMessages(queue, configurations,queue.QueueName);

                    if(result == null)
                        return BadRequest();
                    return Ok(result);
                }
                
            }
            catch (Exception ex)
            {
                
                return BadRequest();
            }

        }

        [HttpPost]
        public async Task<IActionResult> ProcessMealWaiting(
                                [FromServices]RabbitMQConfigurations configurations, 
                                [FromServices]ConnectionFactory factory, 
                                [FromServices]IConnection connection,
                                [FromServices]IConsumeRabbitMQService consumer)
        {
            try{
                var result = await consumer.ProcessQueue("Meal - waiting");

                if(result != null)
                {
                    using (var channel = connection.CreateModel())
                    {
                        var queue = channel.QueueDeclare(queue: "Command Queue",
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);


                        //RabbitMQExtensions.GetAllMessages(queue ,configurations, "Side Dish - waiting"); 
                        
                        string message = JsonConvert.SerializeObject(result);
                        var body = Encoding.UTF8.GetBytes(message);


                        channel.BasicPublish(exchange: "",
                                            routingKey: "Meal - preparing",
                                            basicProperties: null,
                                            body: body);

                        //channel.WaitForConfirmsOrDie(new TimeSpan(0, 0, 5));

                        //Dictionary<string, string> headers = new Dictionary<string, string>();

                        var properties = channel.CreateBasicProperties();

                        properties.Persistent = false;

                        Dictionary<string, object> dictionary = new Dictionary<string, object>();

                        dictionary.Add("Command", "MealPreparing");
                        properties.Headers = dictionary;

                        channel.BasicPublish(exchange: "",
                                            routingKey: "Command Queue",
                                            basicProperties: properties,
                                            body: body);

                        //channel.WaitForConfirmsOrDie(new TimeSpan(0, 0, 5));

                    }
                }

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcessMealPreparing(
                                [FromServices]RabbitMQConfigurations configurations, 
                                [FromServices]ConnectionFactory factory, 
                                [FromServices]IConnection connection,
                                [FromServices]IConsumeRabbitMQService consumer)
        {
            try{
                var result = await consumer.ProcessQueue("Meal - preparing");

                if(result != null)
                {
                    using (var channel = connection.CreateModel())
                    {
                        var queue = channel.QueueDeclare(queue: "Command Queue",
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);


                        //RabbitMQExtensions.GetAllMessages(queue ,configurations, "Side Dish - waiting"); 
                        
                        string message = JsonConvert.SerializeObject(result);
                        var body = Encoding.UTF8.GetBytes(message);


                        /*channel.BasicPublish(exchange: "",
                                            routingKey: "Meal - preparing",
                                            basicProperties: null,
                                            body: body);*/

                        //Dictionary<string, string> headers = new Dictionary<string, string>();

                        var properties = channel.CreateBasicProperties();

                        properties.Persistent = false;

                        Dictionary<string, object> dictionary = new Dictionary<string, object>();

                        dictionary.Add("Command", "MealDone");
                        properties.Headers = dictionary;

                        channel.BasicPublish(exchange: "",
                                            routingKey: "Command Queue",
                                            basicProperties: properties,
                                            body: body);

                    }
                }

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcessSideDishWaiting(
                                [FromServices]RabbitMQConfigurations configurations, 
                                [FromServices]ConnectionFactory factory, 
                                [FromServices]IConnection connection,
                                [FromServices]IConsumeRabbitMQService consumer)
        {
            try{
                var result = await consumer.ProcessQueue("Side Dish - waiting");

                if(result != null)
                {
                    using (var channel = connection.CreateModel())
                    {
                        var queue = channel.QueueDeclare(queue: "Command Queue",
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);


                        //RabbitMQExtensions.GetAllMessages(queue ,configurations, "Side Dish - waiting"); 
                        
                        string message = JsonConvert.SerializeObject(result);
                        var body = Encoding.UTF8.GetBytes(message);


                        channel.BasicPublish(exchange: "",
                                            routingKey: "Side Dish - preparing",
                                            basicProperties: null,
                                            body: body);

                        //Dictionary<string, string> headers = new Dictionary<string, string>();

                        var properties = channel.CreateBasicProperties();

                        properties.Persistent = false;

                        Dictionary<string, object> dictionary = new Dictionary<string, object>();

                        dictionary.Add("Command", "SideDishPreparing");
                        properties.Headers = dictionary;

                        channel.BasicPublish(exchange: "",
                                            routingKey: "Command Queue",
                                            basicProperties: properties,
                                            body: body);

                    }
                }

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcessSideDishPreparing(
                                [FromServices]RabbitMQConfigurations configurations, 
                                [FromServices]ConnectionFactory factory, 
                                [FromServices]IConnection connection,
                                [FromServices]IConsumeRabbitMQService consumer)
        {
            try{
                var result = await consumer.ProcessQueue("Side Dish - preparing");

                if(result != null)
                {
                    using (var channel = connection.CreateModel())
                    {
                        var queue = channel.QueueDeclare(queue: "Command Queue",
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);


                        //RabbitMQExtensions.GetAllMessages(queue ,configurations, "Side Dish - waiting"); 
                        
                        string message = JsonConvert.SerializeObject(result);
                        var body = Encoding.UTF8.GetBytes(message);


                        /*channel.BasicPublish(exchange: "",
                                            routingKey: "Meal - preparing",
                                            basicProperties: null,
                                            body: body);*/

                        //Dictionary<string, string> headers = new Dictionary<string, string>();

                        var properties = channel.CreateBasicProperties();

                        properties.Persistent = false;

                        Dictionary<string, object> dictionary = new Dictionary<string, object>();

                        dictionary.Add("Command", "SideDishDone");
                        properties.Headers = dictionary;

                        channel.BasicPublish(exchange: "",
                                            routingKey: "Command Queue",
                                            basicProperties: properties,
                                            body: body);

                    }
                }

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcessBeverageWaiting(
                                [FromServices]RabbitMQConfigurations configurations, 
                                [FromServices]ConnectionFactory factory, 
                                [FromServices]IConnection connection,
                                [FromServices]IConsumeRabbitMQService consumer)
        {
            try{
                var result = await consumer.ProcessQueue("Beverage - waiting");

                if(result != null)
                {
                    using (var channel = connection.CreateModel())
                    {
                        var queue = channel.QueueDeclare(queue: "Command Queue",
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);


                        //RabbitMQExtensions.GetAllMessages(queue ,configurations, "Side Dish - waiting"); 
                        
                        string message = JsonConvert.SerializeObject(result);
                        var body = Encoding.UTF8.GetBytes(message);


                        channel.BasicPublish(exchange: "",
                                            routingKey: "Beverage - preparing",
                                            basicProperties: null,
                                            body: body);

                        //Dictionary<string, string> headers = new Dictionary<string, string>();

                        var properties = channel.CreateBasicProperties();

                        properties.Persistent = false;

                        Dictionary<string, object> dictionary = new Dictionary<string, object>();

                        dictionary.Add("Command", "BeveragePreparing");
                        properties.Headers = dictionary;

                        channel.BasicPublish(exchange: "",
                                            routingKey: "Command Queue",
                                            basicProperties: properties,
                                            body: body);

                    }
                }

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcessBeveragePreparing(
                                [FromServices]RabbitMQConfigurations configurations, 
                                [FromServices]ConnectionFactory factory, 
                                [FromServices]IConnection connection,
                                [FromServices]IConsumeRabbitMQService consumer)
        {
            try{
                var result = await consumer.ProcessQueue("Beverage - preparing");

                if(result != null)
                {
                    using (var channel = connection.CreateModel())
                    {
                        var queue = channel.QueueDeclare(queue: "Command Queue",
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);


                        //RabbitMQExtensions.GetAllMessages(queue ,configurations, "Side Dish - waiting"); 
                        
                        string message = JsonConvert.SerializeObject(result);
                        var body = Encoding.UTF8.GetBytes(message);


                        /*channel.BasicPublish(exchange: "",
                                            routingKey: "Meal - preparing",
                                            basicProperties: null,
                                            body: body);*/

                        //Dictionary<string, string> headers = new Dictionary<string, string>();

                        var properties = channel.CreateBasicProperties();

                        properties.Persistent = false;

                        Dictionary<string, object> dictionary = new Dictionary<string, object>();

                        dictionary.Add("Command", "BeverageDone");
                        properties.Headers = dictionary;

                        channel.BasicPublish(exchange: "",
                                            routingKey: "Command Queue",
                                            basicProperties: properties,
                                            body: body);

                    }
                }

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcessDesertWaiting(
                                [FromServices]RabbitMQConfigurations configurations, 
                                [FromServices]ConnectionFactory factory, 
                                [FromServices]IConnection connection,
                                [FromServices]IConsumeRabbitMQService consumer)
        {
            try{
                var result = await consumer.ProcessQueue("Desert - waiting");

                if(result != null)
                {
                    using (var channel = connection.CreateModel())
                    {
                        var queue = channel.QueueDeclare(queue: "Command Queue",
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);


                        //RabbitMQExtensions.GetAllMessages(queue ,configurations, "Side Dish - waiting"); 
                        
                        string message = JsonConvert.SerializeObject(result);
                        var body = Encoding.UTF8.GetBytes(message);


                        channel.BasicPublish(exchange: "",
                                            routingKey: "Desert - preparing",
                                            basicProperties: null,
                                            body: body);

                        //Dictionary<string, string> headers = new Dictionary<string, string>();

                        var properties = channel.CreateBasicProperties();

                        properties.Persistent = false;

                        Dictionary<string, object> dictionary = new Dictionary<string, object>();

                        dictionary.Add("Command", "DesertPreparing");
                        properties.Headers = dictionary;

                        channel.BasicPublish(exchange: "",
                                            routingKey: "Command Queue",
                                            basicProperties: properties,
                                            body: body);

                    }
                }

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcessDesertPreparing(
                                [FromServices]RabbitMQConfigurations configurations, 
                                [FromServices]ConnectionFactory factory, 
                                [FromServices]IConnection connection,
                                [FromServices]IConsumeRabbitMQService consumer)
        {
            try{
                var result = await consumer.ProcessQueue("Desert - preparing");

                if(result != null)
                {
                    using (var channel = connection.CreateModel())
                    {
                        var queue = channel.QueueDeclare(queue: "Command Queue",
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);


                        //RabbitMQExtensions.GetAllMessages(queue ,configurations, "Side Dish - waiting"); 
                        
                        string message = JsonConvert.SerializeObject(result);
                        var body = Encoding.UTF8.GetBytes(message);


                        /*channel.BasicPublish(exchange: "",
                                            routingKey: "Meal - preparing",
                                            basicProperties: null,
                                            body: body);*/

                        //Dictionary<string, string> headers = new Dictionary<string, string>();

                        var properties = channel.CreateBasicProperties();

                        properties.Persistent = false;

                        Dictionary<string, object> dictionary = new Dictionary<string, object>();

                        dictionary.Add("Command", "DesertDone");
                        properties.Headers = dictionary;

                        channel.BasicPublish(exchange: "",
                                            routingKey: "Command Queue",
                                            basicProperties: properties,
                                            body: body);

                    }
                }

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
    }
}