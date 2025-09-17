using AutoMapper;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs.Wallet;
using MediatR;

namespace CurrencyExchange.Application.Features.Wallet.Queries.GetWallet
{
    public class GetWalletHandler(
        IWalletRepository walletRepository,
        IMapper mapper
        ) : IRequestHandler<GetWalletQuery, WalletDto>
    {
        public async Task<WalletDto> Handle(GetWalletQuery request, CancellationToken cancellationToken)
        {
            var wallet = await walletRepository.Get(request.GetWalletDto.Id);
            return mapper.Map<WalletDto>(wallet);
        }
    }
}
