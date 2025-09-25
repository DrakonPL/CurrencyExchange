using CurrencyExchange.Application.Features.Wallet.Queries.GetWallet;
using Shouldly;

namespace CurrencyExchange.UnitTests.Features.Wallet
{
    public class GetWalletValidatorTests(TestFixture testFixture) : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task WalletNotFound_Fails404()
        {
            // arrange
            var v = new GetWalletValidator(testFixture.WalletRepository);
            var query = new GetWalletQuery(123456);

            // act
            var result = await v.ValidateAsync(query);

            // assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorCode == "404" && e.PropertyName == "Id");
        }

        [Fact]
        public async Task Valid_Passes()
        {
            // arrange
            var wallet = testFixture.AddWallet("GW-VAL-OK");
            var v = new GetWalletValidator(testFixture.WalletRepository);
            var query = new GetWalletQuery(wallet.Id);

            // act
            var result = await v.ValidateAsync(query);

            // assert
            result.IsValid.ShouldBeTrue();
        }
    }
}