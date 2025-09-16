using CurrencyExchange.Application.DTOs;
using CurrencyExchange.Application.Features.Currency.Queries.GetAllCurrencies;
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
        public async Task<ActionResult<IReadOnlyList<CurrencyDto>>> GetAll()
        {
            var wallets = await _mediator.Send(new GetAllCurrenciesQuery());
            return Ok(wallets);
        }
    }
}
