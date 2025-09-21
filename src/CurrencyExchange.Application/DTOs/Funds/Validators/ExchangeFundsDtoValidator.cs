using CurrencyExchange.Application.Contracts;
using FluentValidation;

namespace CurrencyExchange.Application.DTOs.Funds.Validators
{
    public class ExchangeFundsDtoValidator : AbstractValidator<ExchangeFundsDto>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ICurrencyRepository _currencyRepository;

        public ExchangeFundsDtoValidator(IWalletRepository walletRepository, ICurrencyRepository currencyRepository)
        {
            _walletRepository = walletRepository;
            _currencyRepository = currencyRepository;

            RuleFor(p => p.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            //RuleFor(p => p.WalletId)
            //    .MustAsync(async (walletId, cancellation) =>
            //    {
            //        var wallet = await _walletRepository.Get(walletId);
            //        return wallet != null;
            //    })
            //    .WithErrorCode("404")
            //    .WithMessage("Wallet does not exist.");

            RuleFor(p => p.FromCurrencyCode)
                .MustAsync(async (currencyCode, cancellation) =>
                {
                    var currency = await _currencyRepository.GetByCode(currencyCode);
                    return currency != null;
                })
                .WithErrorCode("404")
                .WithMessage("Currency does not exist.");

            RuleFor(p => p.ToCurrencyCode)
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
