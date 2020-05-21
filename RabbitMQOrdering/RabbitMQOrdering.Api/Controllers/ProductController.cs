using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Services.IServices;

namespace RabbitMQOrdering.Api.Controllers
{
    public class ProductController : BaseController<IProductService, Product>
    {
        public ProductController(IProductService service) : base(service)
        {
        }
    }
}