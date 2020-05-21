using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RabbitMQOrdering.Api.Context;
using RabbitMQOrdering.Api.Repository.IRepository;

namespace RabbitMQOrdering.Api.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected AppDbContext _dbContext;

        public BaseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task Add(T entity) => await _dbContext.Set<T>().AddAsync(entity).ConfigureAwait(true);

        public void Delete(T entity) => _dbContext.Set<T>().Remove(entity);
        public async Task<IEnumerable<T>> GetAll() => await _dbContext.Set<T>().ToListAsync();

        public async Task<T> GetById(int id) => await _dbContext.Set<T>().FindAsync(id).ConfigureAwait(true);
        public async Task Save() => await _dbContext.SaveChangesAsync();
        public void Update(T entity) => _dbContext.Entry(entity).State = EntityState.Modified;
    }
}