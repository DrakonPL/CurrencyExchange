using AutoMapper;
using CurrencyExchange.Application.Common;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyExchange.Application.Features.Currency.Queries.GetAllCurrencies
{
    public class GetAllCurrenciesHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMemoryCache cache
        ) : IRequestHandler<GetAllCurrenciesQuery, List<CurrencyDto>>
    {

        public async Task<List<CurrencyDto>> Handle(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
        {
            var currencyDto = await cache.GetOrCreateAsync(CacheKeys.CurrenciesAll, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheKeys.CacheDuration;

                var currencies = await unitOfWork.CurrencyRepository.GetAll();
                return mapper.Map<List<CurrencyDto>>(currencies);
            });

            return currencyDto ?? [];
        }
    }
}
