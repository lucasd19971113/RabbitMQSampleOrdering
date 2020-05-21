using System.Threading.Tasks;
using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Services;
using RabbitMQOrdering.Api.Shared;

namespace RabbitMQOrdering.Api.Commands
{
    public class ProductDone : ICommand
    {
        private ProductQueue _productQueue;
        public ProductDone(ProductQueue productQueue)
        {
            _productQueue = productQueue;
        }
        public async Task ExecuteAsync()
        {
            await _productQueue.UpdateToDone(_productQueue);
        }
    }
}