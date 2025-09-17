using CurrencyExchange.Application.Contracts;
using FluentValidation;

namespace CurrencyExchange.Application.DTOs.Wallet.Validators
{
    public class GetWalletDtoValidator : AbstractValidator<GetWalletDto>
    {
        private readonly IWalletRepository _walletRepository;

        public GetWalletDtoValidator(IWalletRepository walletRepository)
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
