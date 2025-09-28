using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Infrastructure.Repositories
{
    public class TransactionRepository(CurrencyExchangeDbContext context) : GenericRepository<Transaction>(context), ITransactionRepository
    {
        public async Task<IReadOnlyList<Transaction>> GetByWalletId(int walletId, int? take = null, int? skip = null)
        {
            var query = _context.Transactions
                .AsNoTracking()
                .Include(t => t.Currency)
                .Where(t => t.WalletId == walletId)
                .OrderByDescending(t => t.CreatedAtUtc)
                .AsQueryable();

            if (skip.HasValue) query = query.Skip(skip.Value);
            if (take.HasValue) query = query.Take(take.Value);

            return await query.ToListAsync();
        }
    }
}
