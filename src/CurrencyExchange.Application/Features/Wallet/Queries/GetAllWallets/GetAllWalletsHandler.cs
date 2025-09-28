using AutoMapper;
using CurrencyExchange.Application.Common;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyExchange.Application.Features.Wallet.Queries.GetAllWallets
{
    public class GetAllWalletsHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMemoryCache memoryCache
        ) : IRequestHandler<GetAllWalletsQuery, List<WalletDto>>
    {
        public async Task<List<WalletDto>> Handle(GetAllWalletsQuery request, CancellationToken cancellationToken)
        {
            var walletDto = await memoryCache.GetOrCreateAsync(CacheKeys.WalletsAll, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheKeys.CacheDuration;

                var wallets = await unitOfWork.WalletRepository.GetAll();
                return mapper.Map<List<WalletDto>>(wallets);
            });

            return walletDto ?? [];
        }
    }
}
