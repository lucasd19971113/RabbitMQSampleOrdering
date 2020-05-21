using RabbitMQOrdering.Api.Context;
using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Repository.IRepository;

namespace RabbitMQOrdering.Api.Repository
{
    public class KitchenAreaRepository : BaseRepository<KitchenArea>, IKitchenAreaRepository
    {
        public KitchenAreaRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}