using CurrencyExchange.Application.DTOs.Funds;
using MediatR;

namespace CurrencyExchange.Application.Features.Funds.Commands.WithdrawFunds
{
    public record WithdrawFundsCommand(WithdrawFundsDto WithdrawFundsDto) : IRequest<FundsDto>;
}
