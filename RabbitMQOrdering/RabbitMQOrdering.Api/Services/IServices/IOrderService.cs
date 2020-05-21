using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Shared;

namespace RabbitMQOrdering.Api.Services.IServices
{
    public interface IOrderService : IService<Order>
    {
         Task<Result<Order>> GetOrderByCreatedDate(DateTime createdDate);

         Task<Result<List<Order>>> GetWaitingAndPreparingOrders();
         Task<Result<List<Order>>> GetFullOrdersList();
         Order Create (OrderDto dto);
    }
}