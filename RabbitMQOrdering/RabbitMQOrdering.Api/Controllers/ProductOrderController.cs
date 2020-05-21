using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Services.IServices;

namespace RabbitMQOrdering.Api.Controllers
{
    public class ProductOrderController : BaseController<IProductOrderService, ProductOrder>
    {
        public ProductOrderController(IProductOrderService service) : base(service)
        {
        }
    }
}