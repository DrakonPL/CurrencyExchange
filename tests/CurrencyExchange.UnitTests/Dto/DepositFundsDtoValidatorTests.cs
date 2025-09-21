using CurrencyExchange.Application.DTOs.Funds;
using CurrencyExchange.Application.DTOs.Funds.Validators;
using Shouldly;

namespace CurrencyExchange.UnitTests.Dto
{
    public class DepositFundsDtoValidatorTests(TestFixture testFixture) : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task CurrencyNotFound_Fails404()
        {
            // arrange
            var wallet = testFixture.AddWallet("DV1");
            var v = new DepositFundsDtoValidator(testFixture.WalletRepository, testFixture.CurrencyRepository);
            var dto = new DepositFundsDto { CurrencyCode = "XXX", Amount = 10m };

            // act
            var result = await v.ValidateAsync(dto);

            // assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorCode == "404" && e.PropertyName == "CurrencyCode");
        }

        [Fact]
        public async Task NonPositiveAmount_Fails()
        {
            // arrange
            var wallet = testFixture.AddWallet("DV2");
            var v = new DepositFundsDtoValidator(testFixture.WalletRepository, testFixture.CurrencyRepository);
            var dto = new DepositFundsDto { CurrencyCode = "USD", Amount = 0m };

            // act
            var result = await v.ValidateAsync(dto);

            // assert
            result.IsValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Valid_Passes()
        {
            // arrange
            var wallet = testFixture.AddWallet("DV3");
            var v = new DepositFundsDtoValidator(testFixture.WalletRepository, testFixture.CurrencyRepository);
            var dto = new DepositFundsDto { CurrencyCode = "USD", Amount = 25m };

            // act
            var result = await v.ValidateAsync(dto);

            // assert
            result.IsValid.ShouldBeTrue();
        }
    }
}
