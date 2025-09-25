using CurrencyExchange.Application.DTOs;
using MediatR;

namespace CurrencyExchange.Application.Features.Wallet.Queries.GetWallet
{
    public record GetWalletQuery(int Id) : IRequest<WalletDto>;
}
