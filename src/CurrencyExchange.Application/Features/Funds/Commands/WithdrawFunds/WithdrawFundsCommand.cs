using CurrencyExchange.Application.DTOs.Funds;
using MediatR;

namespace CurrencyExchange.Application.Features.Funds.Commands.WithdrawFunds
{
    public record WithdrawFundsCommand(int Id, WithdrawFundsDto WithdrawFundsDto) : IRequest<FundsDto>;
}
