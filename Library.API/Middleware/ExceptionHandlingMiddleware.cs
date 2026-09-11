using Library.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await HandleAsync(context, e);
            }
        }

        private async Task HandleAsync(HttpContext context, Exception exception)
        {
            _logger.LogError($"Method {context.Request.Method} {context.Request.Path}," +
                $" exception - {exception} with message {exception.Message}");

            var (statusCode, userMessage) = MapExceptionToResponse(exception);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var errorResponse = new
            {
                StatusCode = statusCode,
                UserMessage = userMessage,
                TraceId = context.TraceIdentifier
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }

        private static (int statusCode, string userMessage) MapExceptionToResponse(Exception exception)
        {
            return exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Requested resource was not found"),

                ArgumentException or ArgumentNullException => (StatusCodes.Status400BadRequest, "Invalid request parameters"),

                DbUpdateException => (StatusCodes.Status400BadRequest, "Database operation failed"),

                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };
        }
    }
}
