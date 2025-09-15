using FluentValidation;

namespace CurrencyExchange.Application.DTOs.Wallet.Validators
{
    public class CreateWalletDtoValidator : AbstractValidator<CreateWalletDto>
    {
        public CreateWalletDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Wallet name is required.")
                .MaximumLength(100).WithMessage("Wallet name must not exceed 100 characters.");
        }
    }
}
