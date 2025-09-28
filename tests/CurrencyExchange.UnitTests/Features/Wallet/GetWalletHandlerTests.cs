using CurrencyExchange.Application.Features.Wallet.Queries.GetWallet;
using Shouldly;

namespace CurrencyExchange.UnitTests.Features.Wallet
{
    public class GetWalletHandlerTests(TestFixture testFixture) : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task GetWallet_ReturnsDto_WhenFound()
        {
            // arrange
            var wallet = testFixture.AddWallet("GW-OK");
            var handler = new GetWalletHandler(testFixture.UnitOfWork, testFixture.Mapper, testFixture.MemoryCache);

            // act
            var dto = await handler.Handle(new GetWalletQuery(wallet.Id), CancellationToken.None);

            // assert
            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(wallet.Id);
            dto.Name.ShouldBe("GW-OK");
            dto.Funds.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetWallet_UsesCache_OnSubsequentCalls()
        {
            // arrange
            var wallet = testFixture.AddWallet("GW-CACHE-1");
            var handler = new GetWalletHandler(testFixture.UnitOfWork, testFixture.Mapper, testFixture.MemoryCache);

            // act
            var first = await handler.Handle(new GetWalletQuery(wallet.Id), CancellationToken.None);

            // mutate repository after first read
            wallet.Name = "GW-CACHE-UPDATED";
            testFixture.WalletRepository.Update(wallet);

            // second call should return cached DTO (old name)
            var second = await handler.Handle(new GetWalletQuery(wallet.Id), CancellationToken.None);

            // assert
            first.Name.ShouldBe("GW-CACHE-1");
            second.Name.ShouldBe("GW-CACHE-1");
        }
    }
}