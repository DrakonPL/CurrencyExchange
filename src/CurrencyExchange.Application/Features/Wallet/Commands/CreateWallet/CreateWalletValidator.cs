using CurrencyExchange.Application.Contracts;
using FluentValidation;

namespace CurrencyExchange.Application.Features.Wallet.Commands.CreateWallet
{
    public class CreateWalletValidator : AbstractValidator<CreateWalletCommand>
    {
        public CreateWalletValidator(IWalletRepository walletRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Wallet name is required.")
                .MaximumLength(100).WithMessage("Wallet name must not exceed 100 characters.");

            RuleFor(p => p.Name)
                .MustAsync(async (walletName, cancellation) =>
                {
                    var wallet = await walletRepository.GetByName(walletName);
                    return wallet == null;
                })
                .WithErrorCode("400")
                .WithMessage("Wallet with this name already exist.");
        }
    }
}
