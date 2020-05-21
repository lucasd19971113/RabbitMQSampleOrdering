using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Repository.IRepository;
using RabbitMQOrdering.Api.Services.IServices;
using RabbitMQOrdering.Api.Shared;

namespace RabbitMQOrdering.Api.Services
{
    public class OrderService : BaseService<Order, IOrderRepository>, IOrderService
    {
        public OrderService(IOrderRepository repo) : base(repo)
        {
        }

        public async Task<Result<List<Order>>> GetWaitingAndPreparingOrders()
        {
            var entities = await _repo.GetWaitingAndPreparingOrders();

            if(entities.AsEnumerable().Any())
            {
                return Result.Ok(entities.ToList());
            }

            return Result.Fail<List<Order>>("No register was found", ResultCode.NoContent);
        }

        public async Task<Result<List<Order>>> GetFullOrdersList()
        {
            var entities = await _repo.GetAllOrdersList();

            if(entities.AsEnumerable().Any())
            {
                return Result.Ok(entities.ToList());
            }

            return Result.Fail<List<Order>>("No register was found", ResultCode.NoContent);
        }

        public async Task<Result<Order>> GetOrderByCreatedDate(DateTime createdDate)
        {
            var entity = await _repo.GetOrderByCreatedDate(createdDate);
            if (entity != null)
            {
                return Result.Ok(entity);
            }

            return Result.Fail<Order>("No register was found", ResultCode.NotFound);
        }

        public Order Create(OrderDto orderDto)
        {
            return Order.Create(orderDto);
        }
    }
}