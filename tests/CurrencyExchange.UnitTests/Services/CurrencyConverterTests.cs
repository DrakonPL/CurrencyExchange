using CurrencyExchange.Application.Exceptions;
using Shouldly;

namespace CurrencyExchange.UnitTests.Services
{
    public class CurrencyConverterTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fx;
        public CurrencyConverterTests(TestFixture fx) => _fx = fx;

        [Fact]
        public async Task Convert_SameCurrency_ReturnsSameAmount()
        {
            var result = await _fx.CurrencyConverter.Convert("USD", "USD", 123m);
            result.ShouldBe(123m);
        }

        [Fact]
        public async Task Convert_ThrowsOnNegativeAmount()
        {
            await Should.ThrowAsync<BadRequestException>(() =>
                _fx.CurrencyConverter.Convert("USD", "PLN", -1m));
        }

        [Fact]
        public async Task Convert_UsdToEur_UsesRates()
        {
            // USD rate 4.00, EUR rate 4.40 -> PLN value = 10 * 4.00 = 40 -> /4.40 = 9.0909...
            var result = await _fx.CurrencyConverter.Convert("USD", "EUR", 10m);
            result.ShouldBeInRange(9.0909m, 10.0000m);
        }
    }
}
