using CurrencyExchange.Application.Contracts;
using FluentValidation;

namespace CurrencyExchange.Application.Features.Wallet.Queries.GetWallet
{
    public class GetWalletValidator : AbstractValidator<GetWalletQuery>
    {
        public GetWalletValidator(IUnitOfWork unitOfWork)
        {

            RuleFor(p => p.Id)
                .MustAsync(async (walletId, cancellation) =>
                {
                    var wallet = await unitOfWork.WalletRepository.Get(walletId);
                    return wallet != null;
                })
                .WithErrorCode("404")
                .WithMessage("Wallet does not exist.");
        }
    }
}
