using CurrencyExchange.Application.DTOs.Funds;
using CurrencyExchange.Application.Features.Funds.Commands.ExchangeFunds;
using Shouldly;

namespace CurrencyExchange.UnitTests.Features
{
    public class ExchangeFundsValidatorTests(TestFixture testFixture) : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task WalletNotFound_Fails404()
        {
            // arrange
            var v = new ExchangeFundsValidator(testFixture.WalletRepository, testFixture.CurrencyRepository);
            var dto = new ExchangeFundsDto { FromCurrencyCode = "USD", ToCurrencyCode = "EUR", Amount = 10m };
            var cmd = new ExchangeFundsCommand(987654, dto);

            // act
            var result = await v.ValidateAsync(cmd);

            // assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorCode == "404" && e.PropertyName == "Id");
        }

        [Fact]
        public async Task Valid_Passes()
        {
            // arrange
            var wallet = testFixture.AddWallet("XV-CMD-OK");
            var v = new ExchangeFundsValidator(testFixture.WalletRepository, testFixture.CurrencyRepository);
            var dto = new ExchangeFundsDto { FromCurrencyCode = "USD", ToCurrencyCode = "EUR", Amount = 15m };
            var cmd = new ExchangeFundsCommand(wallet.Id, dto);

            // act
            var result = await v.ValidateAsync(cmd);

            // assert
            result.IsValid.ShouldBeTrue();
        }
    }
}
