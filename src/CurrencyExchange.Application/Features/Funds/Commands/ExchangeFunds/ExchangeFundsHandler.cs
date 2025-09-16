using AutoMapper;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs.Funds;
using CurrencyExchange.Application.DTOs.Funds.Validators;
using FluentValidation;
using MediatR;

namespace CurrencyExchange.Application.Features.Funds.Commands.ExchangeFunds
{
    public class ExchangeFundsHandler(
        IWalletRepository walletRepository,
        ICurrencyRepository currencyRepository,
        IMapper mapper
        ) : IRequestHandler<ExchangeFundsCommand, FundsDto>
    {
        public async Task<FundsDto> Handle(ExchangeFundsCommand request, CancellationToken cancellationToken)
        {
            
            //var validator = new ExchangeFundsDtoValidator(walletRepository, currencyRepository);
            //var validationResult = await validator.ValidateAsync(request.ExchangeFundsDto);

            //if (!validationResult.IsValid)
            //{
            //    throw new ValidationException(validationResult.Errors);
            //}


            var wallet = await walletRepository.Get(request.ExchangeFundsDto.WalletId);
            var fromCurrency = await currencyRepository.GetByCode(request.ExchangeFundsDto.FromCurrencyCode);
            var toCurrency = await currencyRepository.GetByCode(request.ExchangeFundsDto.ToCurrencyCode);

            var funds = wallet.Funds.FirstOrDefault(f => f.CurrencyId == fromCurrency.Id);

            if (funds == null || funds.Amount < request.ExchangeFundsDto.Amount)
            {
                throw new ValidationException("Insufficient funds in the source currency.");
            }

            //withdraw funds from the source currency
            funds.Amount -= request.ExchangeFundsDto.Amount;

            //wymień na PLN
            decimal plnFunds = request.ExchangeFundsDto.Amount * fromCurrency.Rate;

            //wymień na docelową walutę
            decimal exchangedAmount = plnFunds / toCurrency.Rate;

            //check if the wallet already has funds in the target currency
            var targetFunds = wallet.Funds.FirstOrDefault(f => f.CurrencyId == toCurrency.Id);
            if (targetFunds != null)
            {
                targetFunds.Amount += exchangedAmount;
            }
            else
            {
                targetFunds = new Domain.Entities.Funds
                {
                    CurrencyId = toCurrency.Id,
                    Amount = exchangedAmount
                };
                wallet.Funds.Add(targetFunds);
            }

            await walletRepository.Update(wallet);
            return mapper.Map<FundsDto>(targetFunds);
        }
    }
}
