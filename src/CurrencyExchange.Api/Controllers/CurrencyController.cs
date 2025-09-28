using CurrencyExchange.Application.Features.Currency.Queries.GetAllCurrencies;
using CurrencyExchange.Contracts.Currency;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController(IMediator mediator, ILogger<CurrencyController> logger) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<CurrencyResponse>>> GetAll()
        {
            logger.LogInformation("RequestId={RequestId} Currencies_GetAll requested", HttpContext.TraceIdentifier);

            var rates = await mediator.Send(new GetAllCurrenciesQuery());
            var response = rates.Select(r => new CurrencyResponse(r.Code, r.Name, r.Rate)).ToList();

            logger.LogInformation("RequestId={RequestId} Currencies_GetAll succeeded Count={Count}", HttpContext.TraceIdentifier, response.Count);
            return Ok(response);
        }
    }
}
