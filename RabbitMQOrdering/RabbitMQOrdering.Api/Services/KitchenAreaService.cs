using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Repository.IRepository;
using RabbitMQOrdering.Api.Services.IServices;

namespace RabbitMQOrdering.Api.Services
{
    public class KitchenAreaService : BaseService<KitchenArea, IKitchenAreaRepository>, IKitchenAreaService
    {
        public KitchenAreaService(IKitchenAreaRepository repo) : base(repo)
        {
        }
    }
}