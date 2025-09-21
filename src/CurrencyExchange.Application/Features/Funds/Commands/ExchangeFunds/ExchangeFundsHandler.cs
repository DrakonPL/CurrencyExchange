using AutoMapper;
using CurrencyExchange.Application.Common;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs.Funds;
using CurrencyExchange.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyExchange.Application.Features.Funds.Commands.ExchangeFunds
{
    public class ExchangeFundsHandler(
        IWalletRepository walletRepository,
        ICurrencyRepository currencyRepository,
        ICurrencyConverter currencyConverter,
        IMapper mapper,
        IMemoryCache memoryCache
        ) : IRequestHandler<ExchangeFundsCommand, FundsDto>
    {
        public async Task<FundsDto> Handle(ExchangeFundsCommand request, CancellationToken cancellationToken)
        {
            var wallet = await walletRepository.Get(request.Id);
            var fromCurrency = await currencyRepository.GetByCode(request.ExchangeFundsDto.FromCurrencyCode);
            var toCurrency = await currencyRepository.GetByCode(request.ExchangeFundsDto.ToCurrencyCode);

            wallet.WithdrawFunds(fromCurrency, request.ExchangeFundsDto.Amount);

            var exchangedAmount = await currencyConverter.Convert(
                fromCurrency.Code,
                toCurrency.Code,
                request.ExchangeFundsDto.Amount,
                cancellationToken
            );

            var funds = wallet.DepositFunds(toCurrency, exchangedAmount);
            await walletRepository.Update(wallet);

            memoryCache.Remove(CacheKeys.WalletsAll);
            memoryCache.Remove(CacheKeys.WalletById(request.Id));

            return mapper.Map<FundsDto>(funds);
        }
    }
}
