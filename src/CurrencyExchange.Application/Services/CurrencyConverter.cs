using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.Exceptions;
using CurrencyExchange.Application.Interfaces;

namespace CurrencyExchange.Application.Services
{
    public class CurrencyConverter(
        ICurrencyRepository currencyRepository
        ) : ICurrencyConverter
    {
        public async Task<decimal> Convert(string fromCurrencyCode, string toCurrencyCode, decimal amount, CancellationToken ct = default)
        {
            if (amount < 0)
                throw new BadRequestException("Amount must be non-negative.");

            if (string.Equals(fromCurrencyCode, toCurrencyCode, StringComparison.OrdinalIgnoreCase))
                return amount;

            var from = await currencyRepository.GetByCode(fromCurrencyCode);
            var to = await currencyRepository.GetByCode(toCurrencyCode);

            if (from == null || to == null)
                throw new BadRequestException("Currency code not found.");

            // Assuming Rate is mid price in PLN.
            var plnValue = amount * from.Rate;
            var target = plnValue / to.Rate;

            return target;
        }
    }
}
