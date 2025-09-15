using CurrencyExchange.Application.DTOs.Wallet;
using MediatR;

namespace CurrencyExchange.Application.Features.Wallet.Commands.CreateWallet
{
    public record CreateWalletCommand(CreateWalletDto CreateWalletDto) : IRequest<int>;
}
