using CurrencyExchange.Application.Contracts;
using FluentValidation;

namespace CurrencyExchange.Application.Features.Funds.Commands.WithdrawFunds
{
    public class WithdrawFundsValidator : AbstractValidator<WithdrawFundsCommand>
    {
        public WithdrawFundsValidator(IWalletRepository walletRepository)
        {
            RuleFor(x => x.WalletId)
                .MustAsync(async (id, ct) => await walletRepository.Get(id) != null)
                .WithErrorCode("404")
                .WithMessage("Wallet does not exist.");

            RuleFor(p => p.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");
        }
    }
}
