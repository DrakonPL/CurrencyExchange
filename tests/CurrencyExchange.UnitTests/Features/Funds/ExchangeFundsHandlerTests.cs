using CurrencyExchange.Application.Features.Funds.Commands.ExchangeFunds;
using CurrencyExchange.Domain.Exceptions;
using Shouldly;

namespace CurrencyExchange.UnitTests.Features.Funds
{
    public class ExchangeFundsHandlerTests(TestFixture testFixture) : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task Exchange_Insufficient_Throws()
        {
            // arrange
            var wallet = testFixture.AddWallet("E1");
            testFixture.AddFunds(wallet, "USD", 5m);

            var handler = new ExchangeFundsHandler(
                testFixture.UnitOfWork,
                testFixture.CurrencyConverter,
                testFixture.Mapper,
                testFixture.MemoryCache);

            await Should.ThrowAsync<DomainValidationException>(() =>
                handler.Handle(new ExchangeFundsCommand(wallet.Id, "USD", "EUR", 50m), CancellationToken.None));
        }

        [Fact]
        public async Task Exchange_NewTargetCurrency_CreatesFunds()
        {
            // arrange
            var wallet = testFixture.AddWallet("E2");
            testFixture.AddFunds(wallet, "USD", 100m);

            var handler = new ExchangeFundsHandler(
                testFixture.UnitOfWork,
                testFixture.CurrencyConverter,
                testFixture.Mapper,
                testFixture.MemoryCache);

            // act
            var result = await handler.Handle(new ExchangeFundsCommand(wallet.Id, "USD", "EUR", 40m), CancellationToken.None);

            // assert
            result.CurrencyCode.ShouldBe("EUR");

            // 40 USD -> PLN 160 -> EUR at 4.40 ≈ 36.3636
            result.Amount.ShouldBe(36.3636m, 0.0001m);
        }

        [Fact]
        public async Task Exchange_ExistingTarget_Increments()
        {
            // arrange
            var wallet = testFixture.AddWallet("E3");
            testFixture.AddFunds(wallet, "USD", 200m);
            testFixture.AddFunds(wallet, "EUR", 10m);

            var handler = new ExchangeFundsHandler(
                testFixture.UnitOfWork,
                testFixture.CurrencyConverter,
                testFixture.Mapper,
                testFixture.MemoryCache);

            // act
            var result = await handler.Handle(new ExchangeFundsCommand(wallet.Id, "USD", "EUR", 100m), CancellationToken.None);

            // assert
            // 100 USD -> PLN 400 -> EUR ≈ 90.9091 added -> total ≈ 100.9091
            result.Amount.ShouldBe(100.9091m, 0.0002m);
        }
    }
}
