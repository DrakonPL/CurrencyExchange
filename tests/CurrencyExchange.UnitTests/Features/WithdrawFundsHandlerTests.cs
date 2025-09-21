using CurrencyExchange.Application.DTOs.Funds;
using CurrencyExchange.Application.Features.Funds.Commands.WithdrawFunds;
using CurrencyExchange.Common.Exceptions;
using Shouldly;

namespace CurrencyExchange.UnitTests.Features
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
            var dto = new WithdrawFundsDto { CurrencyCode = "USD", Amount = 40m };

            // act
            var result = await handler.Handle(new WithdrawFundsCommand(wallet.Id, dto), CancellationToken.None);

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
            var dto = new WithdrawFundsDto { CurrencyCode = "USD", Amount = 25m };

            // assert
            await Should.ThrowAsync<BadRequestException>(() =>
                handler.Handle(new WithdrawFundsCommand(wallet.Id, dto), CancellationToken.None));
        }
    }
}
