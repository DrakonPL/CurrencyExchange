using CurrencyExchange.Common.Exceptions;
using CurrencyExchange.Domain.Exceptions;
using System.Text.Json;

namespace CurrencyExchange.Api.Middlewares
{
    public class ErrorHandlerMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainValidationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = error switch
                {
                    BadRequestException => StatusCodes.Status400BadRequest,
                    EntityNotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError,
                };

                var result = JsonSerializer.Serialize(new { message = error.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
