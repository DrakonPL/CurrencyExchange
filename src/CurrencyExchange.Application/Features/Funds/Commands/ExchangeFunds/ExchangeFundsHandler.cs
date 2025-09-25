using AutoMapper;
using CurrencyExchange.Application.Common;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs;
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
            var wallet = await walletRepository.Get(request.WalletId);
            var fromCurrency = await currencyRepository.GetByCode(request.FromCurrencyCode);
            var toCurrency = await currencyRepository.GetByCode(request.ToCurrencyCode);

            wallet.WithdrawFunds(fromCurrency, request.Amount);

            var exchangedAmount = await currencyConverter.Convert(
                fromCurrency.Code,
                toCurrency.Code,
                request.Amount,
                cancellationToken
            );

            var funds = wallet.DepositFunds(toCurrency, exchangedAmount);

            await walletRepository.Update(wallet);

            memoryCache.Remove(CacheKeys.WalletsAll);
            memoryCache.Remove(CacheKeys.WalletById(request.WalletId));

            return mapper.Map<FundsDto>(funds);
        }
    }
}
