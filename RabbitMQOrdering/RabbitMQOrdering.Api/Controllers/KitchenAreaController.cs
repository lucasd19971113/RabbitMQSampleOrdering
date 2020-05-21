using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Services.IServices;

namespace RabbitMQOrdering.Api.Controllers
{
    public class KitchenAreaController : BaseController<IKitchenAreaService, KitchenArea>
    {
        public KitchenAreaController(IKitchenAreaService service) : base(service)
        {
        }
    }
}