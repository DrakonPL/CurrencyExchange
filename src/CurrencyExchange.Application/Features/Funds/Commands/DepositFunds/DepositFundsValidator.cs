using CurrencyExchange.Application.Contracts;
using FluentValidation;

namespace CurrencyExchange.Application.Features.Funds.Commands.DepositFunds
{
    public class DepositFundsValidator : AbstractValidator<DepositFundsCommand>
    {
        public DepositFundsValidator(IWalletRepository walletRepository, ICurrencyRepository currencyRepository)
        {
            RuleFor(x => x.WalletId)
                .MustAsync(async (id, ct) => await walletRepository.Get(id) != null)
                .WithErrorCode("404")
                .WithMessage("Wallet does not exist.");

            RuleFor(p => p.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(p => p.CurrencyCode)
                .MustAsync(async (currencyCode, cancellation) =>
                {
                    var currency = await currencyRepository.GetByCode(currencyCode);
                    return currency != null;
                })
                .WithErrorCode("404")
                .WithMessage("Currency does not exist.");
        }
    }
}
