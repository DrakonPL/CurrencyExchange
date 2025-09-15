using AutoMapper;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs.Wallet;
using MediatR;

namespace CurrencyExchange.Application.Features.Wallet.Queries.GetAllWallets
{
    public class GetAllWalletsHandler(
        IWalletRepository walletRepository,
        IMapper mapper
        ) : IRequestHandler<GetAllWalletsQuery, List<WalletDto>>
    {
        public async Task<List<WalletDto>> Handle(GetAllWalletsQuery request, CancellationToken cancellationToken)
        {
            var wallets = await walletRepository.GetAll();
            return mapper.Map<List<WalletDto>>(wallets);
        }
    }
}
