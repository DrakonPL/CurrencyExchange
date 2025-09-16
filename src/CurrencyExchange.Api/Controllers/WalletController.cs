using CurrencyExchange.Application.DTOs.Funds;
using CurrencyExchange.Application.DTOs.Wallet;
using CurrencyExchange.Application.Features.Funds.Commands.DepositFunds;
using CurrencyExchange.Application.Features.Funds.Commands.ExchangeFunds;
using CurrencyExchange.Application.Features.Funds.Commands.WithdrawFunds;
using CurrencyExchange.Application.Features.Wallet.Commands.CreateWallet;
using CurrencyExchange.Application.Features.Wallet.Queries.GetAllWallets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WalletController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<WalletDto>>> GetAll()
        {
            var wallets = await _mediator.Send(new GetAllWalletsQuery());
            return Ok(wallets);
        }

        [HttpPost]
        public async Task<ActionResult<WalletDto>> CreateWallet([FromBody] CreateWalletDto walletDto)
        {
            return Ok(await _mediator.Send(new CreateWalletCommand(walletDto)));
        }

        [HttpPost("deposit")]
        public async Task<ActionResult<FundsDto>> DepositFunds([FromBody] DepositFundsDto fundsDto)
        {
            return Ok(await _mediator.Send(new DepositFundsCommand(fundsDto)));
        }

        [HttpPost("withdraw")]
        public async Task<ActionResult<FundsDto>> WithdrawFunds([FromBody] WithdrawFundsDto fundsDto)
        {
            return Ok(await _mediator.Send(new WithdrawFundsCommand(fundsDto)));
        }

        [HttpPost("exchange")]
        public async Task<ActionResult<FundsDto>> ExchangeFunds([FromBody] ExchangeFundsDto fundsDto)
        {
            return Ok(await _mediator.Send(new ExchangeFundsCommand(fundsDto)));
        }
    }
}
