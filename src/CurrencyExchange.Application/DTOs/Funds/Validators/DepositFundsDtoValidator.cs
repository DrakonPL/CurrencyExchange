using CurrencyExchange.Application.Contracts;
using FluentValidation;

namespace CurrencyExchange.Application.DTOs.Funds.Validators
{
    public class DepositFundsDtoValidator : AbstractValidator<DepositFundsDto>
    {
        private readonly ICurrencyRepository _currencyRepository;

        public DepositFundsDtoValidator(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;

            RuleFor(p => p.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(p => p.CurrencyCode)
                .MustAsync(async (currencyCode, cancellation) =>
                {
                    var currency = await _currencyRepository.GetByCode(currencyCode);
                    return currency != null;
                })
                .WithErrorCode("404")
                .WithMessage("Currency does not exist.");
        }

    }
}
