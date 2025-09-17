using CurrencyExchange.Application.Contracts;
using FluentValidation;

namespace CurrencyExchange.Application.DTOs.Wallet.Validators
{
    public class CreateWalletDtoValidator : AbstractValidator<CreateWalletDto>
    {
        private readonly IWalletRepository _walletRepository;

        public CreateWalletDtoValidator(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Wallet name is required.")
                .MaximumLength(100).WithMessage("Wallet name must not exceed 100 characters.");

            RuleFor(p => p.Name)
                .MustAsync(async (walletName, cancellation) =>
                {
                    var wallet = await _walletRepository.GetByName(walletName);
                    return wallet == null;
                })
                .WithErrorCode("400")
                .WithMessage("Wallet with this name already exist.");
        }
    }
}
