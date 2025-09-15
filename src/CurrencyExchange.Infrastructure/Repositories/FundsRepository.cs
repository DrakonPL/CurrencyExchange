using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Domain.Entities;

namespace CurrencyExchange.Infrastructure.Repositories
{
    public class FundsRepository(CurrencyExchangeDbContext context) : GenericRepository<Funds>(context), IFundsRepository
    {
    }
}
