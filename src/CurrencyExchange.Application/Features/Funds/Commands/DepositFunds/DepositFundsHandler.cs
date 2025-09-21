using AutoMapper;
using CurrencyExchange.Application.Common;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs.Funds;
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
            var wallet = await walletRepository.Get(request.Id);
            var currency = await currencyRepository.GetByCode(request.DepositFundsDto.CurrencyCode);

            var funds = wallet.DepositFunds(currency, request.DepositFundsDto.Amount);
            await walletRepository.Update(wallet);

            memoryCache.Remove(CacheKeys.WalletsAll);
            memoryCache.Remove(CacheKeys.WalletById(request.Id));

            return mapper.Map<FundsDto>(funds);
        }
    }
}
