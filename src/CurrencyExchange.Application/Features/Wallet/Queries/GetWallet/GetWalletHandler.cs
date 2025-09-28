using AutoMapper;
using CurrencyExchange.Application.Common;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyExchange.Application.Features.Wallet.Queries.GetWallet
{
    public class GetWalletHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMemoryCache memoryCache
        ) : IRequestHandler<GetWalletQuery, WalletDto>
    {
        public async Task<WalletDto> Handle(GetWalletQuery request, CancellationToken cancellationToken)
        {
            var walletDto = await memoryCache.GetOrCreateAsync(CacheKeys.WalletById(request.Id), async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheKeys.CacheDuration;

                var wallet = await unitOfWork.WalletRepository.Get(request.Id);
                return mapper.Map<WalletDto>(wallet);
            });

            return walletDto;
        }
    }
}
