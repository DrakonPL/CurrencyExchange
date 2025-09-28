using CurrencyExchange.Application.Exceptions;
using CurrencyExchange.Domain.Exceptions;

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
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "RequestId={RequestId} NotFound {Method} {Path}", context.TraceIdentifier, context.Request.Method, context.Request.Path);
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(new { message = ex.Message });
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex, "RequestId={RequestId} BadRequest {Method} {Path}", context.TraceIdentifier, context.Request.Method, context.Request.Path);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { message = ex.Message });
            }
            catch (DomainValidationException ex)
            {
                _logger.LogWarning(ex, "RequestId={RequestId} DomainValidation {Method} {Path}", context.TraceIdentifier, context.Request.Method, context.Request.Path);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RequestId={RequestId} UnhandledError {Method} {Path}", context.TraceIdentifier, context.Request.Method, context.Request.Path);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { message = "An unexpected error occurred." });
            }
        }
    }
}
