using CurrencyExchange.Application.Features.Funds.Commands.DepositFunds;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;

namespace CurrencyExchange.UnitTests.Features.Funds
{
    public class DepositFundsHandlerTests(TestFixture testFixture) : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task Deposit_NewCurrency_AddsFunds()
        {
            // arrange
            var wallet = testFixture.AddWallet("A");
            var handler = new DepositFundsHandler(testFixture.UnitOfWork, testFixture.Mapper, testFixture.MemoryCache, NullLogger<DepositFundsHandler>.Instance);

            // act
            var result = await handler.Handle(new DepositFundsCommand(wallet.Id, "USD", 50m), CancellationToken.None);

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

            var handler = new DepositFundsHandler(testFixture.UnitOfWork, testFixture.Mapper, testFixture.MemoryCache, NullLogger<DepositFundsHandler>.Instance);

            // act
            var result = await handler.Handle(new DepositFundsCommand(wallet.Id, "USD", 30m), CancellationToken.None);

            // assert
            result.Amount.ShouldBe(50m);
        }
    }
}
