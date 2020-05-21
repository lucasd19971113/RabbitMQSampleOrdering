using System.Collections.Generic;
using System.Threading.Tasks;
using RabbitMQOrdering.Api.Shared;

namespace RabbitMQOrdering.Api.Services.IServices
{
    public interface IService<T>
    {
        Task<Result<T>> GetById(int id);
        Task<Result<List<T>>> GetAll();
        Task<Result> Add(T entity);
        Task<Result> Delete(int id);
        Task<Result> Update(T entity);
    }
}