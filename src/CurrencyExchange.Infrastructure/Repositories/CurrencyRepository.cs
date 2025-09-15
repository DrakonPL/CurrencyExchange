using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Infrastructure.Repositories
{
    public class CurrencyRepository(CurrencyExchangeDbContext context) : GenericRepository<Currency>(context), ICurrencyRepository
    {
        public async Task<Currency> GetByCode(string code)
        {
            return await context.Currencies.FirstOrDefaultAsync(c => c.Code == code);
        }
    }
}
