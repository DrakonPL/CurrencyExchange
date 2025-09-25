using CurrencyExchange.Application.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly CurrencyExchangeDbContext _context;

        public GenericRepository(CurrencyExchangeDbContext context)
        {
            _context = context;
        }

        public async Task<T> Add(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<T> Get(int id)
        {
            return await _context.Set<T>()
                .FindAsync(id);
        }

        public virtual async Task<IReadOnlyList<T>> GetAll()
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
