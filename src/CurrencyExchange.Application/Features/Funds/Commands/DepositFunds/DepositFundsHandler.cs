using AutoMapper;
using CurrencyExchange.Application.Common;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyExchange.Application.Features.Funds.Commands.DepositFunds
{
    public class DepositFundsHandler(
         IWalletRepository walletRepository,
         ICurrencyRepository currencyRepository,
         IMapper mapper,
         IMemoryCache memoryCache
        ) : IRequestHandler<DepositFundsCommand, FundsDto>
    {
        public async Task<FundsDto> Handle(DepositFundsCommand request, CancellationToken cancellationToken)
        {
            var wallet = await walletRepository.Get(request.WalletId);
            var currency = await currencyRepository.GetByCode(request.CurrencyCode);

            var funds = wallet.DepositFunds(currency, request.Amount);
            await walletRepository.Update(wallet);

            memoryCache.Remove(CacheKeys.WalletsAll);
            memoryCache.Remove(CacheKeys.WalletById(request.WalletId));

            return mapper.Map<FundsDto>(funds);
        }
    }
}
