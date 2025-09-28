using CurrencyExchange.Application.Contracts;
using FluentValidation;

namespace CurrencyExchange.Application.Features.Funds.Commands.ExchangeFunds
{
    public class ExchangeFundsValidator : AbstractValidator<ExchangeFundsCommand>
    {
        public ExchangeFundsValidator(IUnitOfWork unitOfWork, ICurrencyRepository currencyRepository)
        {
            RuleFor(x => x.WalletId)
                .MustAsync(async (id, ct) => await unitOfWork.WalletRepository.Get(id) != null)
                .WithErrorCode("404")
                .WithMessage("Wallet does not exist.");

            RuleFor(p => p.FromCurrencyCode)
                .MustAsync(async (currencyCode, cancellation) =>
                {
                    var currency = await currencyRepository.GetByCode(currencyCode);
                    return currency != null;
                })
                .WithErrorCode("404")
                .WithMessage("Currency does not exist.");

            RuleFor(p => p.ToCurrencyCode)
                .MustAsync(async (currencyCode, cancellation) =>
                {
                    var currency = await currencyRepository.GetByCode(currencyCode);
                    return currency != null;
                })
                .WithErrorCode("404")
                .WithMessage("Currency does not exist.");

            RuleFor(p => p.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");
        }
    }
}
