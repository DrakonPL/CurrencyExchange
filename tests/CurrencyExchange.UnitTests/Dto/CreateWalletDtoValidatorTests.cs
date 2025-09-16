using CurrencyExchange.Application.DTOs.Wallet;
using CurrencyExchange.Application.DTOs.Wallet.Validators;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.UnitTests.Dto
{
    public class CreateWalletDtoValidatorTests
    {
        [Fact]
        public async Task EmptyName_Fails()
        {
            var v = new CreateWalletDtoValidator();
            var dto = new CreateWalletDto { Name = "" };
            var result = await v.ValidateAsync(dto);
            result.IsValid.ShouldBeFalse();
        }

        [Fact]
        public async Task TooLongName_Fails()
        {
            var v = new CreateWalletDtoValidator();
            var dto = new CreateWalletDto { Name = new string('x', 101) };
            var result = await v.ValidateAsync(dto);
            result.IsValid.ShouldBeFalse();
        }

        [Fact]
        public async Task ValidName_Passes()
        {
            var v = new CreateWalletDtoValidator();
            var dto = new CreateWalletDto { Name = "My Wallet" };
            var result = await v.ValidateAsync(dto);
            result.IsValid.ShouldBeTrue();
        }
    }
}
