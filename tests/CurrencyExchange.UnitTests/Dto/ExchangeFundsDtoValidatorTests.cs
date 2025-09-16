using CurrencyExchange.Application.DTOs.Funds;
using CurrencyExchange.Application.DTOs.Funds.Validators;
using Shouldly;

namespace CurrencyExchange.UnitTests.Dto
{
    public class ExchangeFundsDtoValidatorTests
    {
        private readonly TestFixture _fx = new();

        [Fact]
        public async Task WalletNotFound_Fails404()
        {
            // arrange
            var v = new ExchangeFundsDtoValidator(_fx.WalletRepository, _fx.CurrencyRepository);
            var dto = new ExchangeFundsDto { WalletId = 9999, FromCurrencyCode = "USD", ToCurrencyCode = "EUR", Amount = 10m };

            // act
            var result = await v.ValidateAsync(dto);

            // assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorCode == "404" && e.PropertyName == "WalletId");
        }

        [Fact]
        public async Task FromCurrencyMissing_Fails404()
        {
            // arrange
            var wallet = _fx.AddWallet("WX1");
            var v = new ExchangeFundsDtoValidator(_fx.WalletRepository, _fx.CurrencyRepository);
            var dto = new ExchangeFundsDto { WalletId = wallet.Id, FromCurrencyCode = "XXX", ToCurrencyCode = "EUR", Amount = 10m };

            // act
            var result = await v.ValidateAsync(dto);

            // assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorCode == "404" && e.PropertyName == "FromCurrencyCode");
        }

        [Fact]
        public async Task ToCurrencyMissing_Fails404()
        {
            // arrange
            var wallet = _fx.AddWallet("WX2");
            var v = new ExchangeFundsDtoValidator(_fx.WalletRepository, _fx.CurrencyRepository);
            var dto = new ExchangeFundsDto { WalletId = wallet.Id, FromCurrencyCode = "USD", ToCurrencyCode = "ZZZ", Amount = 10m };

            // act
            var result = await v.ValidateAsync(dto);

            // assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorCode == "404" && e.PropertyName == "ToCurrencyCode");
        }

        [Fact]
        public async Task NonPositiveAmount_Fails()
        {
            // arrange
            var wallet = _fx.AddWallet("WX3");
            var v = new ExchangeFundsDtoValidator(_fx.WalletRepository, _fx.CurrencyRepository);
            var dto = new ExchangeFundsDto { WalletId = wallet.Id, FromCurrencyCode = "USD", ToCurrencyCode = "EUR", Amount = 0m };

            // act
            var result = await v.ValidateAsync(dto);

            // assert
            result.IsValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Valid_Passes()
        {
            // arrange
            var wallet = _fx.AddWallet("WX4");
            var v = new ExchangeFundsDtoValidator(_fx.WalletRepository, _fx.CurrencyRepository);
            var dto = new ExchangeFundsDto { WalletId = wallet.Id, FromCurrencyCode = "USD", ToCurrencyCode = "EUR", Amount = 15.5m };

            // act
            var result = await v.ValidateAsync(dto);

            // assert
            result.IsValid.ShouldBeTrue();
        }
    }
}
