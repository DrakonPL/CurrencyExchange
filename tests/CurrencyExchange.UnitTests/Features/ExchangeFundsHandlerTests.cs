using CurrencyExchange.Application.DTOs.Funds;
using CurrencyExchange.Application.Exceptions;
using CurrencyExchange.Application.Features.Funds.Commands.ExchangeFunds;
using Shouldly;

namespace CurrencyExchange.UnitTests.Features
{
    public class ExchangeFundsHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fx;
        public ExchangeFundsHandlerTests(TestFixture fx) => _fx = fx;

        [Fact]
        public async Task Exchange_Insufficient_Throws()
        {
            // arrange
            var wallet = _fx.AddWallet("E1");
            _fx.AddFunds(wallet, "USD", 5m);

            var handler = new ExchangeFundsHandler(
                _fx.WalletRepository,
                _fx.CurrencyRepository,
                _fx.CurrencyConverter,
                _fx.Mapper);

            var dto = new ExchangeFundsDto
            {
                WalletId = wallet.Id,
                FromCurrencyCode = "USD",
                ToCurrencyCode = "EUR",
                Amount = 50m
            };

            await Should.ThrowAsync<BadRequestException>(() =>
                handler.Handle(new ExchangeFundsCommand(dto), CancellationToken.None));
        }

        [Fact]
        public async Task Exchange_NewTargetCurrency_CreatesFunds()
        {
            // arrange
            var wallet = _fx.AddWallet("E2");
            _fx.AddFunds(wallet, "USD", 100m);

            var handler = new ExchangeFundsHandler(
                _fx.WalletRepository,
                _fx.CurrencyRepository,
                _fx.CurrencyConverter,
                _fx.Mapper);

            var dto = new ExchangeFundsDto
            {
                WalletId = wallet.Id,
                FromCurrencyCode = "USD",
                ToCurrencyCode = "EUR",
                Amount = 40m
            };

            // act
            var result = await handler.Handle(new ExchangeFundsCommand(dto), CancellationToken.None);

            // assert
            result.CurrencyCode.ShouldBe("EUR");

            // 40 USD -> PLN 160 -> EUR at 4.40 ≈ 36.3636
            result.Amount.ShouldBe(36.3636m, 0.0001m);
        }

        [Fact]
        public async Task Exchange_ExistingTarget_Increments()
        {
            // arrange
            var wallet = _fx.AddWallet("E3");
            _fx.AddFunds(wallet, "USD", 200m);
            _fx.AddFunds(wallet, "EUR", 10m);

            var handler = new ExchangeFundsHandler(
                _fx.WalletRepository,
                _fx.CurrencyRepository,
                _fx.CurrencyConverter,
                _fx.Mapper);

            var dto = new ExchangeFundsDto
            {
                WalletId = wallet.Id,
                FromCurrencyCode = "USD",
                ToCurrencyCode = "EUR",
                Amount = 100m
            };

            // act
            var result = await handler.Handle(new ExchangeFundsCommand(dto), CancellationToken.None);

            // assert
            // 100 USD -> PLN 400 -> EUR ≈ 90.9091 added -> total ≈ 100.9091
            result.Amount.ShouldBe(100.9091m, 0.0002m);
        }
    }
}
