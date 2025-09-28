using CurrencyExchange.Application.Contracts;

namespace CurrencyExchange.Infrastructure
{
    public class UnitOfWork(
        CurrencyExchangeDbContext context,
        ICurrencyRepository currencyRepository,
        IWalletRepository walletRepository,
        IFundsRepository fundsRepository,
        ITransactionRepository transactionRepository) : IUnitOfWork
    {
        private readonly CurrencyExchangeDbContext _context = context;

        public ICurrencyRepository CurrencyRepository { get; private set; } = currencyRepository;

        public IWalletRepository WalletRepository { get; private set; } = walletRepository;

        public IFundsRepository FundsRepository { get; private set; } = fundsRepository;

        public ITransactionRepository TransactionRepository { get; private set; } = transactionRepository;

        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
