using CurrencyExchange.Application.Contracts;
using FluentValidation;

namespace CurrencyExchange.Application.Features.Wallet.Queries.GetWallet
{
    public class GetWalletValidator : AbstractValidator<GetWalletQuery>
    {
        private readonly IWalletRepository _walletRepository;

        public GetWalletValidator(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;

            RuleFor(p => p.Id)
                .MustAsync(async (walletId, cancellation) =>
                {
                    var wallet = await _walletRepository.Get(walletId);
                    return wallet != null;
                })
                .WithErrorCode("404")
                .WithMessage("Wallet does not exist.");
        }
    }
}
