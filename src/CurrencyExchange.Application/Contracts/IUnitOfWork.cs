namespace CurrencyExchange.Application.Contracts
{
    public interface IUnitOfWork
    {
        ICurrencyRepository CurrencyRepository { get; }
        IWalletRepository WalletRepository { get; }

        IFundsRepository FundsRepository { get; }

        Task<int> SaveAsync(CancellationToken cancellationToken);
    }
}
