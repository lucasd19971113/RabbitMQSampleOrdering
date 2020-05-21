using RabbitMQOrdering.Api.Repository.IRepository;
using RabbitMQOrdering.Api.Entities;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RabbitMQOrdering.Api.Repository.IRepository
{
    public interface IOrderRepository : IRepository<Order>
    {
         Task<Order> GetOrderByCreatedDate(DateTime createdDate);
         Task<IEnumerable<Order>> GetWaitingAndPreparingOrders();
         Task<IEnumerable<Order>> GetAllOrdersList();
    }
}