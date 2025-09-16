using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Infrastructure.Repositories
{
    public class WalletRepository(CurrencyExchangeDbContext context) : GenericRepository<Wallet>(context), IWalletRepository
    {
        public override async Task<Wallet> Get(int id)
        {
            return await _context.Wallets
                .Include(w => w.Funds)
                .ThenInclude(f => f.Currency)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public override async Task<IReadOnlyList<Wallet>> GetAll()
        {
            return await _context.Wallets
                .Include(w => w.Funds)
                .ThenInclude(f => f.Currency)
                .ToListAsync();
        }
    }
}
