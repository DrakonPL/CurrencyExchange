using CurrencyExchange.Application.DTOs.Funds;
using MediatR;

namespace CurrencyExchange.Application.Features.Funds.Commands.ExchangeFunds
{
    public record ExchangeFundsCommand(int Id, ExchangeFundsDto ExchangeFundsDto) : IRequest<FundsDto>;
}
