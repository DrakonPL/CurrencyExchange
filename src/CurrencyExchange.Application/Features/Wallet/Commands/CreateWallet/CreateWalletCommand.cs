using MediatR;

namespace CurrencyExchange.Application.Features.Wallet.Commands.CreateWallet
{
    public record CreateWalletCommand(string Name) : IRequest<int>;
}
