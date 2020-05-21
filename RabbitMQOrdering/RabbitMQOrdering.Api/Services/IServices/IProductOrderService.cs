using System.Threading.Tasks;
using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Shared;

namespace RabbitMQOrdering.Api.Services.IServices
{
    public interface IProductOrderService : IService<ProductOrder>
    {
         Task<Result<ProductOrder>> GetByOrderAndProductId(int id, int orderId, int productId);
    }
}