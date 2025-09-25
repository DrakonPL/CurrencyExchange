using CurrencyExchange.Application.Features.Wallet.Commands.CreateWallet;
using Shouldly;

namespace CurrencyExchange.UnitTests.Features.Wallet
{
    public class CreateWalletValidatorTests(TestFixture testFixture) : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task EmptyName_Fails()
        {
            // arrange
            var v = new CreateWalletValidator(testFixture.WalletRepository);
            var cmd = new CreateWalletCommand(string.Empty);

            // act
            var result = await v.ValidateAsync(cmd);

            // assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.PropertyName == "Name" && e.ErrorMessage.Contains("required"));
        }

        [Fact]
        public async Task NameTooLong_Fails()
        {
            // arrange
            var v = new CreateWalletValidator(testFixture.WalletRepository);
            var longName = new string('A', 101);
            var cmd = new CreateWalletCommand(longName);

            // act
            var result = await v.ValidateAsync(cmd);

            // assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.PropertyName == "Name" && e.ErrorMessage.Contains("not exceed 100"));
        }

        [Fact]
        public async Task DuplicateName_Fails400()
        {
            // arrange
            var existing = testFixture.AddWallet("CW-EXIST");
            var v = new CreateWalletValidator(testFixture.WalletRepository);
            var cmd = new CreateWalletCommand("CW-EXIST");

            // act
            var result = await v.ValidateAsync(cmd);

            // assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorCode == "400" && e.PropertyName == "Name");
        }

        [Fact]
        public async Task Valid_Passes()
        {
            // arrange
            var v = new CreateWalletValidator(testFixture.WalletRepository);
            var cmd = new CreateWalletCommand("CW-VALID");

            // act
            var result = await v.ValidateAsync(cmd);

            // assert
            result.IsValid.ShouldBeTrue();
        }
    }
}