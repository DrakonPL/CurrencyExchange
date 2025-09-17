using CurrencyExchange.Application.DTOs.Funds;
using CurrencyExchange.Application.DTOs.Funds.Validators;
using Shouldly;

namespace CurrencyExchange.UnitTests.Dto
{
    public class WithdrawFundsDtoValidatorTests(TestFixture testFixture) : IClassFixture<TestFixture>
    {

        [Fact]
        public async Task WalletNotFound_FailsWith404()
        {
            // arrange
            var v = new WithdrawFundsDtoValidator(testFixture.WalletRepository);
            var dto = new WithdrawFundsDto { WalletId = 9999, CurrencyCode = "USD", Amount = 10m };

            // act
            var result = await v.ValidateAsync(dto);

            // assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorCode == "404");
        }

        [Fact]
        public async Task NonPositiveAmount_Fails()
        {
            // arrange
            var wallet = testFixture.AddWallet("W1");
            var v = new WithdrawFundsDtoValidator(testFixture.WalletRepository);
            var dto = new WithdrawFundsDto { WalletId = wallet.Id, CurrencyCode = "USD", Amount = 0m };

            // act
            var result = await v.ValidateAsync(dto);

            // assert
            result.IsValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Valid_Passes()
        {
            // arrange
            var wallet = testFixture.AddWallet("W2");
            var v = new WithdrawFundsDtoValidator(testFixture.WalletRepository);
            var dto = new WithdrawFundsDto { WalletId = wallet.Id, CurrencyCode = "USD", Amount = 5m };

            // act
            var result = await v.ValidateAsync(dto);

            // assert
            result.IsValid.ShouldBeTrue();
        }
    }
}
