using System;
using System.Collections.Generic;
using System.Linq;

namespace RabbitMQOrdering.Api.Entities
{
    public class Order : BaseEntity
    {
        public ICollection<ProductOrder> ProductOrder { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public int PointOfSaleId { get; set; }
        public PointOfSale PointOfSale { get; set; }
        public decimal Total { get; set; }
        public static Order order;
        public static Order Create (OrderDto dto) {
                order = new Order {

                PointOfSaleId = dto.PointOfSaleId,
                
                ProductOrder = dto.ProductOrder?.Select (x => new ProductOrder { ProductId = x.ProductId, ProductQueueStatus = ProductOrderStatus.Waiting, CreatedAt = DateTime.Now }).ToList (),

                CreatedAt = DateTime.Now,

                UpdatedAt = null,

                OrderStatus = OrderStatusEnum.Waiting
                
            };
            
            if (dto.ProductOrder.Count > 0) 
                 setOrder();

                GetTotalPrice(dto.ProductOrder);
               
            return order;
        }

        public static void GetTotalPrice(List<ProductOrderDto> orders)
        {
            decimal price = 0.0m;
            if(orders.AsEnumerable().Any())
            {
                orders.ForEach(o =>{
                    order.Total += o.Price;
                });
            }
        }

        public static void setOrder () {
            foreach (var item in order.ProductOrder)
            {
                item.Order = order;
            }
        }
    }

    public class OrderDto 
    {
        public int PointOfSaleId { get; set; }
        public List<ProductOrderDto> ProductOrder { get; set; } = new List<ProductOrderDto>();
    }

    public enum OrderStatusEnum
    {
        Waiting = 1,
        Preparing = 2,
        Done = 3,
        Delivered = 4,
        Cancelled = 5
    }
}