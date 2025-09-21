using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs.Funds.Validators;
using FluentValidation;

namespace CurrencyExchange.Application.Features.Funds.Commands.ExchangeFunds
{
    public class ExchangeFundsValidator : AbstractValidator<ExchangeFundsCommand>
    {
        public ExchangeFundsValidator(IWalletRepository walletRepository, ICurrencyRepository currencyRepository)
        {
            RuleFor(x => x.Id)
                .MustAsync(async (id, ct) => await walletRepository.Get(id) != null)
                .WithErrorCode("404")
                .WithMessage("Wallet does not exist.");

            RuleFor(x => x.ExchangeFundsDto)
                .SetValidator(new ExchangeFundsDtoValidator(walletRepository, currencyRepository));
        }
    }
}
