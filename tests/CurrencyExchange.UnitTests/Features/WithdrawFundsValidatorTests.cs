using CurrencyExchange.Application.DTOs.Funds;
using CurrencyExchange.Application.Features.Funds.Commands.WithdrawFunds;
using Shouldly;

namespace CurrencyExchange.UnitTests.Features
{
    public class WithdrawFundsValidatorTests(TestFixture testFixture) : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task WalletNotFound_Fails404()
        {
            // arrange
            var v = new WithdrawFundsValidator(testFixture.WalletRepository);
            var dto = new WithdrawFundsDto { CurrencyCode = "USD", Amount = 10m };
            var cmd = new WithdrawFundsCommand(123456, dto);

            // act
            var result = await v.ValidateAsync(cmd);

            // assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorCode == "404" && e.PropertyName == "Id");
        }

        [Fact]
        public async Task NonPositiveAmount_Fails()
        {
            // arrange
            var wallet = testFixture.AddWallet("WV-CMD-1");
            var v = new WithdrawFundsValidator(testFixture.WalletRepository);
            var dto = new WithdrawFundsDto { CurrencyCode = "USD", Amount = 0m };
            var cmd = new WithdrawFundsCommand(wallet.Id, dto);

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
            var v = new WithdrawFundsValidator(testFixture.WalletRepository);
            var dto = new WithdrawFundsDto { CurrencyCode = "USD", Amount = 5m };
            var cmd = new WithdrawFundsCommand(wallet.Id, dto);

            // act
            var result = await v.ValidateAsync(cmd);

            // assert
            result.IsValid.ShouldBeTrue();
        }
    }
}
