using CurrencyExchange.Application.Features.Wallet.Commands.CreateWallet;
using Shouldly;

namespace CurrencyExchange.UnitTests.Features.Wallet
{
    public class CreateWalletHandlerTests(TestFixture testFixture) : IClassFixture<TestFixture>
    {

        [Fact]
        public async Task CreateWallet_Succeeds_ReturnsId()
        {
            // arrange
            var handler = new CreateWalletHandler(testFixture.WalletRepository, testFixture.MemoryCache);

            // act
            var id = await handler.Handle(new CreateWalletCommand("My Wallet"), CancellationToken.None);
            var stored = await testFixture.WalletRepository.Get(id);

            // assert
            id.ShouldBeGreaterThan(0);
            stored!.Name.ShouldBe("My Wallet");
        }
    }
}
