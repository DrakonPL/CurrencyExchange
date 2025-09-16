using CurrencyExchange.Application.DTOs.Funds;
using CurrencyExchange.Application.Features.Funds.Commands.DepositFunds;
using Shouldly;

namespace CurrencyExchange.UnitTests.Features
{
    public class DepositFundsHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fx;
        public DepositFundsHandlerTests(TestFixture fx) => _fx = fx;

        [Fact]
        public async Task Deposit_NewCurrency_AddsFunds()
        {
            // arrange
            var wallet = _fx.AddWallet("A");
            var handler = new DepositFundsHandler(_fx.WalletRepository, _fx.CurrencyRepository, _fx.Mapper);
            var dto = new DepositFundsDto { WalletId = wallet.Id, CurrencyCode = "USD", Amount = 50m };

            // act
            var result = await handler.Handle(new DepositFundsCommand(dto), CancellationToken.None);

            // assert
            result.Amount.ShouldBe(50m);
            result.CurrencyCode.ShouldBe("USD");
        }

        [Fact]
        public async Task Deposit_ExistingCurrency_IncrementsAmount()
        {
            // arrange
            var wallet = _fx.AddWallet("B");
            _fx.AddFunds(wallet, "USD", 20m);

            var handler = new DepositFundsHandler(_fx.WalletRepository, _fx.CurrencyRepository, _fx.Mapper);
            var dto = new DepositFundsDto { WalletId = wallet.Id, CurrencyCode = "USD", Amount = 30m };

            // act
            var result = await handler.Handle(new DepositFundsCommand(dto), CancellationToken.None);

            // assert
            result.Amount.ShouldBe(50m);
        }
    }
}
