using CurrencyExchange.Application.Contracts;
using FluentValidation;

namespace CurrencyExchange.Application.DTOs.Funds.Validators
{
    public class WithdrawFundsDtoValidator : AbstractValidator<WithdrawFundsDto>
    {
        private readonly IWalletRepository _walletRepository;

        public WithdrawFundsDtoValidator(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;

            RuleFor(p => p.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(p => p.WalletId)
                .MustAsync(async (walletId, cancellation) =>
                {
                    var wallet = await _walletRepository.Get(walletId);
                    return wallet != null;
                })
                .WithErrorCode("404")
                .WithMessage("Wallet does not exist.");
        }
    }
}
