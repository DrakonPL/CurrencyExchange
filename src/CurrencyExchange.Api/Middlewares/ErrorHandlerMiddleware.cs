using CurrencyExchange.Application.Exceptions;
using CurrencyExchange.Domain.Exceptions;
using System.Text.Json;

namespace CurrencyExchange.Api.Middlewares
{
    public class ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger = logger;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainValidationException ex)
            {
                _logger.LogWarning(ex, "Domain validation failed for {Method} {Path} (RequestId: {RequestId})",
                    context.Request.Method, context.Request.Path, context.TraceIdentifier);

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Unhandled exception for {Method} {Path} (RequestId: {RequestId})",
                    context.Request.Method, context.Request.Path, context.TraceIdentifier);

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
