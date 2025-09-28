using CurrencyExchange.Application.Contracts;
using FluentValidation;

namespace CurrencyExchange.Application.Features.Wallet.Queries.GetWalletTransactions
{
    public class GetWalletTransactionsValidator : AbstractValidator<GetWalletTransactionsQuery>
    {
        public GetWalletTransactionsValidator(IWalletRepository walletRepository)
        {
            RuleFor(p => p.WalletId)
                .MustAsync(async (walletId, cancellation) =>
                {
                    var wallet = await walletRepository.Get(walletId);
                    return wallet != null;
                })
                .WithErrorCode("404")
                .WithMessage("Wallet does not exist.");
        }
    }
}
