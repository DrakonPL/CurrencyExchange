using CurrencyExchange.Application.DTOs.Funds;
using CurrencyExchange.Application.Features.Funds.Commands.WithdrawFunds;
using FluentValidation;
using Shouldly;

namespace CurrencyExchange.UnitTests.Features
{
    public class WithdrawFundsHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fx;
        public WithdrawFundsHandlerTests(TestFixture fx) => _fx = fx;

        [Fact]
        public async Task Withdraw_Valid_DecreasesAmount()
        {
            // arrange
            var wallet = _fx.AddWallet("W1");
            _fx.AddFunds(wallet, "USD", 100m);

            var handler = new WithdrawFundsHandler(_fx.WalletRepository, _fx.CurrencyRepository, _fx.Mapper);
            var dto = new WithdrawFundsDto { WalletId = wallet.Id, CurrencyCode = "USD", Amount = 40m };

            // act
            var result = await handler.Handle(new WithdrawFundsCommand(dto), CancellationToken.None);

            // assert
            result.Amount.ShouldBe(60m);
        }

        [Fact]
        public async Task Withdraw_Insufficient_ThrowsValidation()
        {
            // arrange
            var wallet = _fx.AddWallet("W2");
            _fx.AddFunds(wallet, "USD", 10m);

            var handler = new WithdrawFundsHandler(_fx.WalletRepository, _fx.CurrencyRepository, _fx.Mapper);
            var dto = new WithdrawFundsDto { WalletId = wallet.Id, CurrencyCode = "USD", Amount = 25m };

            // assert
            await Should.ThrowAsync<ValidationException>(() =>
                handler.Handle(new WithdrawFundsCommand(dto), CancellationToken.None));
        }
    }
}
