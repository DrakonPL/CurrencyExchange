using CurrencyExchange.Application.DTOs;
using MediatR;

namespace CurrencyExchange.Application.Features.Wallet.Queries.GetAllWallets
{
    public record GetAllWalletsQuery : IRequest<List<WalletDto>>;
}
