using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace TravelAppAPI.Infrastructure
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError("{Message}", exception.Message);
            var details = new ProblemDetails
            {
                Title = "API Error",
                Status = StatusCodes.Status500InternalServerError,
                Detail = $"API Error {exception.Message}",
                Instance = "API"
            };
            var response = JsonSerializer.Serialize(details);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsync(response, cancellationToken);
            return true;


        }
    }
}
