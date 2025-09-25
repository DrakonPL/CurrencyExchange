using CurrencyExchange.Application.Features.Funds.Commands.ExchangeFunds;
using Shouldly;

namespace CurrencyExchange.UnitTests.Features.Funds
{
    public class ExchangeFundsValidatorTests(TestFixture testFixture) : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task WalletNotFound_Fails404()
        {
            // arrange
            var v = new ExchangeFundsValidator(testFixture.WalletRepository, testFixture.CurrencyRepository);
            var cmd = new ExchangeFundsCommand(987654, "USD", "EUR", 10m);

            // act
            var result = await v.ValidateAsync(cmd);

            // assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorCode == "404" && e.PropertyName == "WalletId");
        }

        [Fact]
        public async Task Valid_Passes()
        {
            // arrange
            var wallet = testFixture.AddWallet("XV-CMD-OK");
            var v = new ExchangeFundsValidator(testFixture.WalletRepository, testFixture.CurrencyRepository);
            var cmd = new ExchangeFundsCommand(wallet.Id, "USD", "EUR", 15m);

            // act
            var result = await v.ValidateAsync(cmd);

            // assert
            result.IsValid.ShouldBeTrue();
        }
    }
}
