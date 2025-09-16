using CurrencyExchange.Application.DTOs.Funds;
using CurrencyExchange.Application.DTOs.Funds.Validators;
using Shouldly;


namespace CurrencyExchange.UnitTests.Dto
{
    public class DepositFundsDtoValidatorTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fx = new();

        [Fact]
        public async Task WalletNotFound_Fails404()
        {
            var v = new DepositFundsDtoValidator(_fx.WalletRepository, _fx.CurrencyRepository);
            var dto = new DepositFundsDto { WalletId = 9999, CurrencyCode = "USD", Amount = 10m };
            var result = await v.ValidateAsync(dto);
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorCode == "404" && e.PropertyName == "WalletId");
        }

        [Fact]
        public async Task CurrencyNotFound_Fails404()
        {
            var wallet = _fx.AddWallet("DV1");
            var v = new DepositFundsDtoValidator(_fx.WalletRepository, _fx.CurrencyRepository);
            var dto = new DepositFundsDto { WalletId = wallet.Id, CurrencyCode = "XXX", Amount = 10m };
            var result = await v.ValidateAsync(dto);
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorCode == "404" && e.PropertyName == "CurrencyCode");
        }

        [Fact]
        public async Task NonPositiveAmount_Fails()
        {
            var wallet = _fx.AddWallet("DV2");
            var v = new DepositFundsDtoValidator(_fx.WalletRepository, _fx.CurrencyRepository);
            var dto = new DepositFundsDto { WalletId = wallet.Id, CurrencyCode = "USD", Amount = 0m };
            var result = await v.ValidateAsync(dto);
            result.IsValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Valid_Passes()
        {
            var wallet = _fx.AddWallet("DV3");
            var v = new DepositFundsDtoValidator(_fx.WalletRepository, _fx.CurrencyRepository);
            var dto = new DepositFundsDto { WalletId = wallet.Id, CurrencyCode = "USD", Amount = 25m };
            var result = await v.ValidateAsync(dto);
            result.IsValid.ShouldBeTrue();
        }
    }
}
