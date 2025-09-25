using CurrencyExchange.Application.DTOs;
using MediatR;

namespace CurrencyExchange.Application.Features.Funds.Commands.WithdrawFunds
{
    public record WithdrawFundsCommand(int WalletId, string CurrencyCode, decimal Amount) : IRequest<FundsDto>;
}
