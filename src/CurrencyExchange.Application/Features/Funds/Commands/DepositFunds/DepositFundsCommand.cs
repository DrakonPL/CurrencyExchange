using CurrencyExchange.Application.DTOs;
using MediatR;

namespace CurrencyExchange.Application.Features.Funds.Commands.DepositFunds
{
    public record DepositFundsCommand(int WalletId, string CurrencyCode, decimal Amount) : IRequest<FundsDto>;
}
