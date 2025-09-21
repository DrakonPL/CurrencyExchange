using CurrencyExchange.Application.DTOs.Funds;
using CurrencyExchange.Application.Features.Funds.Commands.DepositFunds;
using Shouldly;

namespace CurrencyExchange.UnitTests.Features
{
    public class DepositFundsHandlerTests(TestFixture testFixture) : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task Deposit_NewCurrency_AddsFunds()
        {
            // arrange
            var wallet = testFixture.AddWallet("A");
            var handler = new DepositFundsHandler(testFixture.WalletRepository, testFixture.CurrencyRepository, testFixture.Mapper, testFixture.MemoryCache);
            var dto = new DepositFundsDto { CurrencyCode = "USD", Amount = 50m };

            // act
            var result = await handler.Handle(new DepositFundsCommand(wallet.Id, dto), CancellationToken.None);

            // assert
            result.Amount.ShouldBe(50m);
            result.CurrencyCode.ShouldBe("USD");
        }

        [Fact]
        public async Task Deposit_ExistingCurrency_IncrementsAmount()
        {
            // arrange
            var wallet = testFixture.AddWallet("B");
            testFixture.AddFunds(wallet, "USD", 20m);

            var handler = new DepositFundsHandler(testFixture.WalletRepository, testFixture.CurrencyRepository, testFixture.Mapper, testFixture.MemoryCache);
            var dto = new DepositFundsDto { CurrencyCode = "USD", Amount = 30m };

            // act
            var result = await handler.Handle(new DepositFundsCommand(wallet.Id, dto), CancellationToken.None);

            // assert
            result.Amount.ShouldBe(50m);
        }
    }
}
