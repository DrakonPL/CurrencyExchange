using FluentValidation;

namespace CurrencyExchange.Application.DTOs.Funds.Validators
{
    public class WithdrawFundsDtoValidator : AbstractValidator<WithdrawFundsDto>
    {
        public WithdrawFundsDtoValidator()
        {
            RuleFor(p => p.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");
        }
    }
}
