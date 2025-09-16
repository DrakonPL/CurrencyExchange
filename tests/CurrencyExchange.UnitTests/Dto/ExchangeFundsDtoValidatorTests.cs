using CurrencyExchange.Application.DTOs.Funds;
using CurrencyExchange.Application.DTOs.Funds.Validators;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.UnitTests.Dto
{
    public class ExchangeFundsDtoValidatorTests
    {
        private readonly TestFixture _fx = new();

        [Fact]
        public async Task WalletNotFound_Fails404()
        {
            var v = new ExchangeFundsDtoValidator(_fx.WalletRepository, _fx.CurrencyRepository);
            var dto = new ExchangeFundsDto { WalletId = 9999, FromCurrencyCode = "USD", ToCurrencyCode = "EUR", Amount = 10m };
            var result = await v.ValidateAsync(dto);
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorCode == "404" && e.PropertyName == "WalletId");
        }

        [Fact]
        public async Task FromCurrencyMissing_Fails404()
        {
            var wallet = _fx.AddWallet("WX1");
            var v = new ExchangeFundsDtoValidator(_fx.WalletRepository, _fx.CurrencyRepository);
            var dto = new ExchangeFundsDto { WalletId = wallet.Id, FromCurrencyCode = "XXX", ToCurrencyCode = "EUR", Amount = 10m };
            var result = await v.ValidateAsync(dto);
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorCode == "404" && e.PropertyName == "FromCurrencyCode");
        }

        [Fact]
        public async Task ToCurrencyMissing_Fails404()
        {
            var wallet = _fx.AddWallet("WX2");
            var v = new ExchangeFundsDtoValidator(_fx.WalletRepository, _fx.CurrencyRepository);
            var dto = new ExchangeFundsDto { WalletId = wallet.Id, FromCurrencyCode = "USD", ToCurrencyCode = "ZZZ", Amount = 10m };
            var result = await v.ValidateAsync(dto);
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorCode == "404" && e.PropertyName == "ToCurrencyCode");
        }

        [Fact]
        public async Task NonPositiveAmount_Fails()
        {
            var wallet = _fx.AddWallet("WX3");
            var v = new ExchangeFundsDtoValidator(_fx.WalletRepository, _fx.CurrencyRepository);
            var dto = new ExchangeFundsDto { WalletId = wallet.Id, FromCurrencyCode = "USD", ToCurrencyCode = "EUR", Amount = 0m };
            var result = await v.ValidateAsync(dto);
            result.IsValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Valid_Passes()
        {
            var wallet = _fx.AddWallet("WX4");
            var v = new ExchangeFundsDtoValidator(_fx.WalletRepository, _fx.CurrencyRepository);
            var dto = new ExchangeFundsDto { WalletId = wallet.Id, FromCurrencyCode = "USD", ToCurrencyCode = "EUR", Amount = 15.5m };
            var result = await v.ValidateAsync(dto);
            result.IsValid.ShouldBeTrue();
        }
    }
}
