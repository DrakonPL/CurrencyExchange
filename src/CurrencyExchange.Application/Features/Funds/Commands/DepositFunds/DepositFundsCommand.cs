using CurrencyExchange.Application.DTOs.Funds;
using MediatR;

namespace CurrencyExchange.Application.Features.Funds.Commands.DepositFunds
{
    public record DepositFundsCommand(int Id, DepositFundsDto DepositFundsDto) : IRequest<FundsDto>;
}
