using AutoMapper;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs;
using MediatR;

namespace CurrencyExchange.Application.Features.Currency.Queries.GetAllCurrencies
{
    public class GetAllCurrenciesHandler(
        ICurrencyRepository currencyRepository,
        IMapper mapper
        ) : IRequestHandler<GetAllCurrenciesQuery, List<CurrencyDto>>
    {
        public async Task<List<CurrencyDto>> Handle(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
        {
            var currencies = await currencyRepository.GetAll();
            return mapper.Map<List<CurrencyDto>>(currencies);
        }
    }
}
