using CurrencyExchange.Application.DTOs;
using MediatR;

namespace CurrencyExchange.Application.Features.Currency.Queries.GetAllCurrencies
{
    public record GetAllCurrenciesQuery() : IRequest<List<CurrencyDto>>;
}
