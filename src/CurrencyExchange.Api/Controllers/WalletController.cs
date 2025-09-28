using CurrencyExchange.Application.Features.Funds.Commands.DepositFunds;
using CurrencyExchange.Application.Features.Funds.Commands.ExchangeFunds;
using CurrencyExchange.Application.Features.Funds.Commands.WithdrawFunds;
using CurrencyExchange.Application.Features.Wallet.Commands.CreateWallet;
using CurrencyExchange.Application.Features.Wallet.Queries.GetAllWallets;
using CurrencyExchange.Application.Features.Wallet.Queries.GetWallet;
using CurrencyExchange.Application.Features.Wallet.Queries.GetWalletTransactions;
using CurrencyExchange.Contracts.Funds;
using CurrencyExchange.Contracts.Wallet;
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
        public async Task<ActionResult<IReadOnlyList<WalletResponse>>> GetAll()
        {
            var wallets = await _mediator.Send(new GetAllWalletsQuery());
            var response = wallets.Select(w => new WalletResponse(w.Id, w.Name, w.Funds.Select(f => new FundsResponse(f.CurrencyCode, f.Amount)).ToList())).ToList();    
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WalletResponse>> GetWallet(int id)
        {
            var wallet = await _mediator.Send(new GetWalletQuery(id));
            var response = new WalletResponse(wallet.Id, wallet.Name, wallet.Funds.Select(f => new FundsResponse(f.CurrencyCode, f.Amount)).ToList());
            return Ok(wallet);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateWalletResponse>> CreateWallet([FromBody] CreateWalletRequest request)
        {
            var id = await _mediator.Send(new CreateWalletCommand(request.Name));
            return Ok(new CreateWalletResponse(id, request.Name));
        }

        [HttpPost("{id}/deposit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FundsResponse>> DepositFunds(int id,[FromBody] DepositFundsRequest request)
        {
            var funds = await _mediator.Send(new DepositFundsCommand(id, request.CurrencyCode, request.Amount));
            var response = new FundsResponse(funds.CurrencyCode, funds.Amount);
            return Ok(response);
        }

        [HttpPost("{id}/withdraw")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FundsResponse>> WithdrawFunds(int id,[FromBody] WithdrawFundsRequest request)
        {
            var funds = await _mediator.Send(new WithdrawFundsCommand(id, request.CurrencyCode, request.Amount));
            var response = new FundsResponse(funds.CurrencyCode, funds.Amount);
            return Ok(response);
        }

        [HttpPost("{id}/exchange")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FundsResponse>> ExchangeFunds(int id,[FromBody] ExchangeFundsRequest request)
        {
            var funds = await _mediator.Send(new ExchangeFundsCommand(id, request.FromCurrencyCode, request.ToCurrencyCode, request.Amount));  
            var response = new FundsResponse(funds.CurrencyCode, funds.Amount);
            return Ok(response);
        }

        [HttpGet("{id}/transactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<TransactionResponse>>> GetTransactions(int id, [FromQuery] int? take, [FromQuery] int? skip)
        {
            var items = await _mediator.Send(new GetWalletTransactionsQuery(id, take, skip));
            var resp = items.Select(t => new TransactionResponse(
                t.Id,
                t.WalletId,
                t.CurrencyCode,
                t.Type,
                t.Direction,
                t.Amount,
                t.RateAtTransaction,
                t.CreatedAtUtc,
                t.CorrelationId
            )).ToList();
            return Ok(resp);
        }
    }
}
