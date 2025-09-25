using CurrencyExchange.Application.Features.Funds.Commands.DepositFunds;
using Shouldly;

namespace CurrencyExchange.UnitTests.Features.Funds
{
    public class DepositFundsValidatorTests(TestFixture testFixture) : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task WalletNotFound_Fails404()
        {
            // arrange
            var v = new DepositFundsValidator(testFixture.WalletRepository, testFixture.CurrencyRepository);
            var cmd = new DepositFundsCommand(999999, "USD", 10m);

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
            var wallet = testFixture.AddWallet("DV-CMD-OK");
            var v = new DepositFundsValidator(testFixture.WalletRepository, testFixture.CurrencyRepository);
            var cmd = new DepositFundsCommand(wallet.Id, "USD", 25m);

            // act
            var result = await v.ValidateAsync(cmd);

            // assert
            result.IsValid.ShouldBeTrue();
        }
    }
}
