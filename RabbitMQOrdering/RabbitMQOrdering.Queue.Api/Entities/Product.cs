using System;
using System.Collections.Generic;
using System.Linq;

namespace RabbitMQOrdering.Queue.Api.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int KitchenAreaId { get; set; }
        public KitchenArea KitchenArea { get; set; }
        public ICollection<ProductOrder> ProductOrder { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductQueueDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string KitchenAreaName { get; set; }

        public static ProductQueueDto CreateProductQueueDto (
            Order order, 
            Product product, 
            KitchenArea kitchenArea,
            int position) =>
            new ProductQueueDto
            {
                Id = order.ProductOrder.ToList()[position].Id,

                Name = product.Name,

                OrderId = order.Id,
                    
                ProductId = product.Id,

                KitchenAreaName = kitchenArea.Name
            };
    }

    public class ProductQueue 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string KitchenAreaName { get; set; }
    }

    //"[{\"payload_bytes\":86,\"redelivered\":false,\"exchange\":\"\",\"routing_key\":\"Side Dish - waiting\",\"message_count\":3,\"properties\":[],\"payload\":\"{\\\"Id\\\":1,\\\"Name\\\":\\\"French Fries\\\",\\\"OrderId\\\":1,\\\"ProductId\\\":1,\\\"KitchenAreaName\\\":\\\"Side Dish\\\"}\",\"payload_encoding\":\"string\"},{\"payload_bytes\":86,\"redelivered\":false,\"exchange\":\"\",\"routing_key\":\"Side Dish - waiting\",\"message_count\":2,\"properties\":[],\"payload\":\"{\\\"Id\\\":2,\\\"Name\\\":\\\"French Fries\\\",\\\"OrderId\\\":1,\\\"ProductId\\\":1,\\\"KitchenAreaName\\\":\\\"Side Dish\\\"}\",\"payload_encoding\":\"string\"},{\"payload_bytes\":86,\"redelivered\":false,\"exchange\":\"\",\"routing_key\":\"Side Dish - waiting\",\"message_count\":1,\"properties\":[],\"payload\":\"{\\\"Id\\\":3,\\\"Name\\\":\\\"French Fries\\\",\\\"OrderId\\\":1,\\\"ProductId\\\":1,\\\"KitchenAreaName\\\":\\\"Side Dish\\\"}\",\"payload_encoding\":\"string\"},{\"payload_bytes\":86,\"redelivered\":false,\"exchange\":\"\",\"routing_key\":\"Side Dish - waiting\",\"message_count\":0,\"properties\":[],\"payload\":\"{\\\"Id\\\":4,\\\"Name\\\":\\\"French Fries\\\",\\\"OrderId\\\":1,\\\"ProductId\\\":1,\\\"KitchenAreaName\\\":\\\"Side Dish\\\"}\",\"payload_encoding\":\"string\"}]"

    public class QueuePayload
    {
        public int payload_bytes { get; set; }
        public bool redelivered { get; set; }
        public string routing_key { get; set; }
        public string payload { get; set; }
    }

}