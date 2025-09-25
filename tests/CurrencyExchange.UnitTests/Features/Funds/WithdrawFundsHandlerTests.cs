using CurrencyExchange.Application.Features.Funds.Commands.WithdrawFunds;
using CurrencyExchange.Domain.Exceptions;
using Shouldly;

namespace CurrencyExchange.UnitTests.Features.Funds
{
    public class WithdrawFundsHandlerTests(TestFixture testFixture) : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task Withdraw_Valid_DecreasesAmount()
        {
            // arrange
            var wallet = testFixture.AddWallet("W1");
            testFixture.AddFunds(wallet, "USD", 100m);

            var handler = new WithdrawFundsHandler(testFixture.WalletRepository, testFixture.CurrencyRepository, testFixture.Mapper, testFixture.MemoryCache);

            // act
            var result = await handler.Handle(new WithdrawFundsCommand(wallet.Id, "USD", 40m), CancellationToken.None);

            // assert
            result.Amount.ShouldBe(60m);
        }

        [Fact]
        public async Task Withdraw_Insufficient_ThrowsValidation()
        {
            // arrange
            var wallet = testFixture.AddWallet("W2");
            testFixture.AddFunds(wallet, "USD", 10m);

            var handler = new WithdrawFundsHandler(testFixture.WalletRepository, testFixture.CurrencyRepository, testFixture.Mapper, testFixture.MemoryCache);

            // assert
            await Should.ThrowAsync<DomainValidationException>(() =>
                handler.Handle(new WithdrawFundsCommand(wallet.Id, "USD", 25m), CancellationToken.None));
        }
    }
}
