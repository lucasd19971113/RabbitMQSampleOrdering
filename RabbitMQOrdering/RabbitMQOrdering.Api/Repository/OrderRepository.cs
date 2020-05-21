using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RabbitMQOrdering.Api.Context;
using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Repository.IRepository;

namespace RabbitMQOrdering.Api.Repository
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task Add(Order order)
        {
            var entity = new Order
            {
                PointOfSaleId = order.PointOfSaleId,

                CreatedAt = order.CreatedAt,

                UpdatedAt = order.UpdatedAt,

                OrderStatus = order.OrderStatus,

                Total = order.Total
            };

            var productOrderEntities = new List<ProductOrder>();

            order.ProductOrder.ToList().ForEach(po => {
                productOrderEntities.Add(po);
            });

            

            await _dbContext.Orders.AddAsync(order);

            await _dbContext.ProductOrders.AddRangeAsync(productOrderEntities);

            await this.Save();
        }

        public async Task<Order> GetOrderByCreatedDate(DateTime createdDate) => 
            await _dbContext.Orders.Where(o => o.CreatedAt == createdDate).FirstOrDefaultAsync();

        public async Task<IEnumerable<Order>> GetWaitingAndPreparingOrders() => await _dbContext.Orders
            .Include(po => po.ProductOrder)
            .Select(o => new Order{
                Id = o.Id,
                PointOfSaleId = o.PointOfSaleId,
                ProductOrder = o.ProductOrder.Select(d => new ProductOrder {
                            Id = d.Id,
                            ProductId = d.ProductId,
                            OrderId = d.OrderId,
                            ProductQueueStatus = d.ProductQueueStatus
                        }).ToList(),
                OrderStatus = o.OrderStatus,
                Total = o.Total

            }).Where(o => o.OrderStatus == OrderStatusEnum.Waiting || o.OrderStatus == OrderStatusEnum.Preparing)
            .ToListAsync();

        
        public async Task<IEnumerable<Order>> GetAllOrdersList() => await _dbContext.Orders
            .Include(po => po.ProductOrder)
            .Select(o => new Order{
                Id = o.Id,
                PointOfSaleId = o.PointOfSaleId,
                ProductOrder = o.ProductOrder.Select(d => new ProductOrder {
                            Id = d.Id,
                            ProductId = d.ProductId,
                            OrderId = d.OrderId,
                            ProductQueueStatus = d.ProductQueueStatus
                        }).ToList(),
                OrderStatus = o.OrderStatus,
                Total = o.Total

            })
            .ToListAsync();
        
    }
}