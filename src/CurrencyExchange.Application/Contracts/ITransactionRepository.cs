using CurrencyExchange.Domain.Entities;

namespace CurrencyExchange.Application.Contracts
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<IReadOnlyList<Transaction>> GetByWalletId(int walletId, int? take = null, int? skip = null);
    }
}
