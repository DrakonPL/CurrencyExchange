using CurrencyExchange.Application.DTOs.Funds;
using MediatR;

namespace CurrencyExchange.Application.Features.Funds.Commands.ExchangeFunds
{
    public record ExchangeFundsCommand(ExchangeFundsDto ExchangeFundsDto) : IRequest<FundsDto>;
}
