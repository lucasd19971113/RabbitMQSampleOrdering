using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RabbitMQOrdering.Api.Context;
using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Repository.IRepository;

namespace RabbitMQOrdering.Api.Repository
{
    public class ProductOrderRepository : BaseRepository<ProductOrder>, IProductOrderRepository
    {
        public ProductOrderRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ProductOrder> GetByOrderAndProductId(int id, int orderId, int productId) => 
            await _dbContext.ProductOrders.Where(po => po.Id == id && po.OrderId == orderId && po.ProductId == productId)
            .FirstOrDefaultAsync();
    }
}