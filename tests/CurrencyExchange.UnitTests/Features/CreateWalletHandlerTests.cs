using CurrencyExchange.Application.DTOs.Wallet;
using CurrencyExchange.Application.DTOs.Wallet.Validators;
using CurrencyExchange.Application.Features.Wallet.Commands.CreateWallet;
using Shouldly;

namespace CurrencyExchange.UnitTests.Features
{
    public class CreateWalletHandlerTests(TestFixture testFixture) : IClassFixture<TestFixture>
    {

        [Fact]
        public async Task CreateWallet_Succeeds_ReturnsId()
        {
            // arrange
            var handler = new CreateWalletHandler(testFixture.WalletRepository, testFixture.MemoryCache);
            var dto = new CreateWalletDto { Name = "My Wallet" };

            // act
            var id = await handler.Handle(new CreateWalletCommand(dto), CancellationToken.None);
            var stored = await testFixture.WalletRepository.Get(id);

            // assert
            id.ShouldBeGreaterThan(0);
            stored!.Name.ShouldBe("My Wallet");
        }

        [Fact]
        public async Task Validator_Direct_TooLong_Fails()
        {
            // arrange
            var v = new CreateWalletDtoValidator(testFixture.WalletRepository);
            var dto = new CreateWalletDto { Name = new string('x', 150) };

            // act
            var result = await v.ValidateAsync(dto);

            // assert
            result.IsValid.ShouldBeFalse();
        }
    }
}
