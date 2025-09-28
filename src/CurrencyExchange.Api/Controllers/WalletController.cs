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
    public class WalletController(IMediator mediator, ILogger<WalletController> logger) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<WalletResponse>>> GetAll()
        {
            logger.LogInformation("RequestId={RequestId} Wallets_GetAll requested", HttpContext.TraceIdentifier);

            var wallets = await mediator.Send(new GetAllWalletsQuery());
            var response = wallets.Select(w => new WalletResponse(w.Id, w.Name, w.Funds.Select(f => new FundsResponse(f.CurrencyCode, f.Amount)).ToList())).ToList();

            logger.LogInformation("RequestId={RequestId} Wallets_GetAll succeeded Count={Count}", HttpContext.TraceIdentifier, response.Count);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WalletResponse>> GetWallet(int id)
        {
            logger.LogInformation("RequestId={RequestId} Wallet_Get requested WalletId={WalletId}", HttpContext.TraceIdentifier, id);

            var wallet = await mediator.Send(new GetWalletQuery(id));
            var response = new WalletResponse(wallet.Id, wallet.Name, wallet.Funds.Select(f => new FundsResponse(f.CurrencyCode, f.Amount)).ToList());

            logger.LogInformation("RequestId={RequestId} Wallet_Get succeeded WalletId={WalletId} FundsCount={FundsCount}", HttpContext.TraceIdentifier, id, response.Funds.Count);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateWalletResponse>> CreateWallet([FromBody] CreateWalletRequest request)
        {
            logger.LogInformation("RequestId={RequestId} Wallet_Create requested Name={Name}", HttpContext.TraceIdentifier, request.Name);

            var id = await mediator.Send(new CreateWalletCommand(request.Name));

            logger.LogInformation("RequestId={RequestId} Wallet_Create succeeded WalletId={WalletId} Name={Name}", HttpContext.TraceIdentifier, id, request.Name);
            return Ok(new CreateWalletResponse(id, request.Name));
        }

        [HttpPost("{id}/deposit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FundsResponse>> DepositFunds(int id,[FromBody] DepositFundsRequest request)
        {
            logger.LogInformation("RequestId={RequestId} Wallet_Deposit requested WalletId={WalletId} Currency={Currency} Amount={Amount}", HttpContext.TraceIdentifier, id, request.CurrencyCode, request.Amount);

            var funds = await mediator.Send(new DepositFundsCommand(id, request.CurrencyCode, request.Amount));
            var response = new FundsResponse(funds.CurrencyCode, funds.Amount);

            logger.LogInformation("RequestId={RequestId} Wallet_Deposit succeeded WalletId={WalletId} Currency={Currency} Amount={Amount} NewAmount={NewAmount}", HttpContext.TraceIdentifier, id, response.CurrencyCode, request.Amount, response.Amount);
            return Ok(response);
        }

        [HttpPost("{id}/withdraw")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FundsResponse>> WithdrawFunds(int id,[FromBody] WithdrawFundsRequest request)
        {
            logger.LogInformation("RequestId={RequestId} Wallet_Withdraw requested WalletId={WalletId} Currency={Currency} Amount={Amount}", HttpContext.TraceIdentifier, id, request.CurrencyCode, request.Amount);

            var funds = await mediator.Send(new WithdrawFundsCommand(id, request.CurrencyCode, request.Amount));
            var response = new FundsResponse(funds.CurrencyCode, funds.Amount);

            logger.LogInformation("RequestId={RequestId} Wallet_Withdraw succeeded WalletId={WalletId} Currency={Currency} Amount={Amount} NewAmount={NewAmount}", HttpContext.TraceIdentifier, id, response.CurrencyCode, request.Amount, response.Amount);
            return Ok(response);
        }

        [HttpPost("{id}/exchange")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FundsResponse>> ExchangeFunds(int id,[FromBody] ExchangeFundsRequest request)
        {
            logger.LogInformation("RequestId={RequestId} Wallet_Exchange requested WalletId={WalletId} From={FromCurrency} To={ToCurrency} Amount={Amount}", HttpContext.TraceIdentifier, id, request.FromCurrencyCode, request.ToCurrencyCode, request.Amount);

            var funds = await mediator.Send(new ExchangeFundsCommand(id, request.FromCurrencyCode, request.ToCurrencyCode, request.Amount));  
            var response = new FundsResponse(funds.CurrencyCode, funds.Amount);

            logger.LogInformation("RequestId={RequestId} Wallet_Exchange succeeded WalletId={WalletId} From={FromCurrency} To={ToCurrency} Amount={Amount} CreditedCurrency={CreditedCurrency} CreditedAmount={CreditedAmount}", HttpContext.TraceIdentifier, id, request.FromCurrencyCode, request.ToCurrencyCode, request.Amount, response.CurrencyCode, response.Amount);
            return Ok(response);
        }

        [HttpGet("{id}/transactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<TransactionResponse>>> GetTransactions(int id, [FromQuery] int? take, [FromQuery] int? skip)
        {
            logger.LogInformation("RequestId={RequestId} Wallet_Transactions requested WalletId={WalletId} Take={Take} Skip={Skip}", HttpContext.TraceIdentifier, id, take, skip);

            var items = await mediator.Send(new GetWalletTransactionsQuery(id, take, skip));
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

            logger.LogInformation("RequestId={RequestId} Wallet_Transactions succeeded WalletId={WalletId} Count={Count}", HttpContext.TraceIdentifier, id, resp.Count);
            return Ok(resp);
        }
    }
}
