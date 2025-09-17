using CurrencyExchange.Domain.Entities;

namespace CurrencyExchange.Application.Contracts
{
    public interface IWalletRepository : IGenericRepository<Wallet>
    {
        Task<Wallet> GetByName(string name);
    }
}
