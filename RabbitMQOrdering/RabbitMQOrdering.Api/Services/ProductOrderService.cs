using System.Threading.Tasks;
using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Repository.IRepository;
using RabbitMQOrdering.Api.Services.IServices;
using RabbitMQOrdering.Api.Shared;

namespace RabbitMQOrdering.Api.Services
{
    public class ProductOrderService : BaseService<ProductOrder, IProductOrderRepository>, IProductOrderService
    {
        public ProductOrderService(IProductOrderRepository repo) : base(repo)
        {
        }

        public async Task<Result<ProductOrder>> GetByOrderAndProductId(int id, int orderId, int productId)
        {
            var entity = await _repo.GetByOrderAndProductId(id, orderId, productId);
            if (entity != null)
            {
                return Result.Ok(entity);
            }

            return Result.Fail<ProductOrder>("Nenhum registro com o ID encontrado", ResultCode.NotFound);
        }

        
    }
}