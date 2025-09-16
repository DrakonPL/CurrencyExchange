using CurrencyExchange.Application.DTOs.Wallet;
using CurrencyExchange.Application.DTOs.Wallet.Validators;
using Shouldly;

namespace CurrencyExchange.UnitTests.Dto
{
    public class CreateWalletDtoValidatorTests
    {
        [Fact]
        public async Task EmptyName_Fails()
        {
            // arrange
            var v = new CreateWalletDtoValidator();
            var dto = new CreateWalletDto { Name = "" };

            // act
            var result = await v.ValidateAsync(dto);

            // assert
            result.IsValid.ShouldBeFalse();
        }

        [Fact]
        public async Task TooLongName_Fails()
        {
            // arrange
            var v = new CreateWalletDtoValidator();
            var dto = new CreateWalletDto { Name = new string('x', 101) };

            // act
            var result = await v.ValidateAsync(dto);

            // assert
            result.IsValid.ShouldBeFalse();
        }

        [Fact]
        public async Task ValidName_Passes()
        {
            // arrange
            var v = new CreateWalletDtoValidator();
            var dto = new CreateWalletDto { Name = "My Wallet" };

            // act
            var result = await v.ValidateAsync(dto);

            // assert
            result.IsValid.ShouldBeTrue();
        }
    }
}
