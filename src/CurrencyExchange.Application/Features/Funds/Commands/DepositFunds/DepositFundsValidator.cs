using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs.Funds.Validators;
using FluentValidation;

namespace CurrencyExchange.Application.Features.Funds.Commands.DepositFunds
{
    public class DepositFundsValidator : AbstractValidator<DepositFundsCommand>
    {
        public DepositFundsValidator(IWalletRepository walletRepository, ICurrencyRepository currencyRepository)
        {
            RuleFor(x => x.Id)
                .MustAsync(async (id, ct) => await walletRepository.Get(id) != null)
                .WithErrorCode("404")
                .WithMessage("Wallet does not exist.");

            RuleFor(x => x.DepositFundsDto)
                .SetValidator(new DepositFundsDtoValidator(currencyRepository));
        }
    }
}
