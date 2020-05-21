using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Repository.IRepository;
using RabbitMQOrdering.Api.Services.IServices;

namespace RabbitMQOrdering.Api.Services
{
    public class ProductService : BaseService<Product, IProductRepository>, IProductService
    {
        public ProductService(IProductRepository repo) : base(repo)
        {
        }
    }
}