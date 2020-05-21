using System.Threading.Tasks;
using RabbitMQOrdering.Api.Entities;

namespace RabbitMQOrdering.Api.Repository.IRepository
{
    public interface IProductOrderRepository : IRepository<ProductOrder>
    {
         Task<ProductOrder> GetByOrderAndProductId(int id, int orderId, int productId);
    }
}