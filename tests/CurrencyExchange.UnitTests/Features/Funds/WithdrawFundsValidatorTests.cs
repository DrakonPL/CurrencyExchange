using CurrencyExchange.Application.Features.Funds.Commands.WithdrawFunds;
using Shouldly;

namespace CurrencyExchange.UnitTests.Features.Funds
{
    public class WithdrawFundsValidatorTests(TestFixture testFixture) : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task WalletNotFound_Fails404()
        {
            // arrange
            var v = new WithdrawFundsValidator(testFixture.UnitOfWork);
            var cmd = new WithdrawFundsCommand(123456, "USD", 10m);

            // act
            var result = await v.ValidateAsync(cmd);

            // assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorCode == "404" && e.PropertyName == "WalletId");
        }

        [Fact]
        public async Task NonPositiveAmount_Fails()
        {
            // arrange
            var wallet = testFixture.AddWallet("WV-CMD-1");
            var v = new WithdrawFundsValidator(testFixture.UnitOfWork);
            var cmd = new WithdrawFundsCommand(wallet.Id, "USD", 0m);

            // act
            var result = await v.ValidateAsync(cmd);

            // assert
            result.IsValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Valid_Passes()
        {
            // arrange
            var wallet = testFixture.AddWallet("WV-CMD-OK");
            var v = new WithdrawFundsValidator(testFixture.UnitOfWork);
            var cmd = new WithdrawFundsCommand(wallet.Id, "USD", 5m);

            // act
            var result = await v.ValidateAsync(cmd);

            // assert
            result.IsValid.ShouldBeTrue();
        }
    }
}
