using CurrencyExchange.Application.DTOs;
using MediatR;

namespace CurrencyExchange.Application.Features.Funds.Commands.ExchangeFunds
{
    public record ExchangeFundsCommand(
        int WalletId,
        string FromCurrencyCode,
        string ToCurrencyCode,
        decimal Amount) : IRequest<FundsDto>;
}
