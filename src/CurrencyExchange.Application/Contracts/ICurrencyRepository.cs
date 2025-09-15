using CurrencyExchange.Domain.Entities;

namespace CurrencyExchange.Application.Contracts
{
    public interface ICurrencyRepository : IGenericRepository<Currency>
    {
        Task<Currency> GetByCode(string code);
    }
}
