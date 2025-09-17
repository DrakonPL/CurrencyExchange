using CurrencyExchange.Application.DTOs.Wallet;
using MediatR;

namespace CurrencyExchange.Application.Features.Wallet.Queries.GetWallet
{
    public record GetWalletQuery(GetWalletDto GetWalletDto) : IRequest<WalletDto>;
}
