using WeatherApp.Application.Interfaces;

namespace WeatherApp.Api.Middleware
{
    /// <summary>
    /// Middleware that simulates upstream service failures every Nth request.
    /// This is configurable and only applies to specific endpoints.
    /// </summary>
    public class SimulatedFailureMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SimulatedFailureMiddleware> _logger;
        private readonly int _failureInterval;

        public SimulatedFailureMiddleware(
            RequestDelegate next,
            ILogger<SimulatedFailureMiddleware> logger,
            int failureInterval = 5)
        {
            _next = next;
            _logger = logger;
            _failureInterval = failureInterval;
        }

        public async Task InvokeAsync(HttpContext context, IRequestCounter requestCounter)
        {
            // Only apply to weather endpoint
            if (!context.Request.Path.StartsWithSegments("/weather"))
            {
                await _next(context);
                return;
            }

            var requestNumber = requestCounter.IncrementAndGet();

            if (requestNumber % _failureInterval == 0)
            {
                _logger.LogWarning("Simulating upstream failure for request #{RequestNumber}", requestNumber);

                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { error = "Service temporarily unavailable. Please try again later." });
                return;
            }

            await _next(context);
        }
    }

    public static class SimulatedFailureMiddlewareExtensions
    {
        public static IApplicationBuilder UseSimulatedFailure(this IApplicationBuilder builder, int failureInterval = 5)
        {
            return builder.UseMiddleware<SimulatedFailureMiddleware>(failureInterval);
        }
    }
}
