using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs.Funds.Validators;
using FluentValidation;

namespace CurrencyExchange.Application.Features.Funds.Commands.WithdrawFunds
{
    public class WithdrawFundsValidator : AbstractValidator<WithdrawFundsCommand>
    {
        public WithdrawFundsValidator(IWalletRepository walletRepository)
        {
            RuleFor(x => x.Id)
                .MustAsync(async (id, ct) => await walletRepository.Get(id) != null)
                .WithErrorCode("404")
                .WithMessage("Wallet does not exist.");

            RuleFor(x => x.WithdrawFundsDto)
                .SetValidator(new WithdrawFundsDtoValidator(walletRepository));
        }
    }
}
