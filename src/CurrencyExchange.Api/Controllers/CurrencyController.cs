using CurrencyExchange.Application.Features.Currency.Queries.GetAllCurrencies;
using CurrencyExchange.Contracts.Currency;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CurrencyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CurrencyResponse>>> GetAll()
        {
            var rates = await _mediator.Send(new GetAllCurrenciesQuery());
            var response = rates.Select(r => new CurrencyResponse(r.Code, r.Name, r.Rate)).ToList();
            return Ok(response);
        }
    }
}
