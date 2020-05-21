using System.Threading.Tasks;
using RabbitMQOrdering.Api.Shared;

namespace RabbitMQOrdering.Api.Commands
{
    public interface ICommand
    {
        Task ExecuteAsync();
    }
}