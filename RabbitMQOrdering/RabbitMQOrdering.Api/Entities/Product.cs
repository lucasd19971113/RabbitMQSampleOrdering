using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQOrdering.Api.Services;
using RabbitMQOrdering.Api.Services.IServices;
using RabbitMQOrdering.Api.Shared;

namespace RabbitMQOrdering.Api.Entities
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

    public class ProductQueue : ProductQueueService
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string KitchenAreaName { get; set; }
        public IServiceProvider _serviceScopefactory;

        public ProductQueue(IServiceProvider _serviceScopefactory) : base(_serviceScopefactory)
        {
        }
    }

}