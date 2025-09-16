using CurrencyExchange.Application.DTOs.Funds;
using CurrencyExchange.Application.Features.Funds.Commands.DepositFunds;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.UnitTests.Features
{
    public class DepositFundsHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fx;
        public DepositFundsHandlerTests(TestFixture fx) => _fx = fx;

        [Fact]
        public async Task Deposit_NewCurrency_AddsFunds()
        {
            var wallet = _fx.AddWallet("A");
            var handler = new DepositFundsHandler(_fx.WalletRepository, _fx.CurrencyRepository, _fx.Mapper);

            var dto = new DepositFundsDto { WalletId = wallet.Id, CurrencyCode = "USD", Amount = 50m };
            var result = await handler.Handle(new DepositFundsCommand(dto), CancellationToken.None);

            result.Amount.ShouldBe(50m);
            result.CurrencyCode.ShouldBe("USD");
        }

        [Fact]
        public async Task Deposit_ExistingCurrency_IncrementsAmount()
        {
            var wallet = _fx.AddWallet("B");
            _fx.AddFunds(wallet, "USD", 20m);

            var handler = new DepositFundsHandler(_fx.WalletRepository, _fx.CurrencyRepository, _fx.Mapper);
            var dto = new DepositFundsDto { WalletId = wallet.Id, CurrencyCode = "USD", Amount = 30m };
            var result = await handler.Handle(new DepositFundsCommand(dto), CancellationToken.None);

            result.Amount.ShouldBe(50m);
        }
    }
}
