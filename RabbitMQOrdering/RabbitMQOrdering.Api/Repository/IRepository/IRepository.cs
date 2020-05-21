using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RabbitMQOrdering.Api.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task Add(T entity);
        Task Save();
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        void Delete(T entity);
        void Update(T entity);
    }
}