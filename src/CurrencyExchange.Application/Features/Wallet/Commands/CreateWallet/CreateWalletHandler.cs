using CurrencyExchange.Application.Common;
using CurrencyExchange.Application.Contracts;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyExchange.Application.Features.Wallet.Commands.CreateWallet
{
    public class CreateWalletHandler(
            IWalletRepository walletRepository,
            IMemoryCache memoryCache
        ) : IRequestHandler<CreateWalletCommand, int>
    {
        public async Task<int> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            var wallet = new Domain.Entities.Wallet
            {
                Name = request.Name
            };

            wallet = await walletRepository.Add(wallet);
            memoryCache.Remove(CacheKeys.WalletsAll);

            return wallet.Id;
        }
    }
}
