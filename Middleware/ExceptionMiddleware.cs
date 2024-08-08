using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_course_api.Middleware
{
    /// <summary>
    /// Represents an exception middleware.
    /// </summary>
    /// <param name="next">
    /// The next request delegate.
    /// </param>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <param name="env">
    /// The host environment.
    /// </param>
    public class ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env
    )
    {
        /// <summary>
        /// The next request delegate.
        /// </summary>
        private readonly RequestDelegate _next = next;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<ExceptionMiddleware> _logger = logger;

        /// <summary>
        /// The host environment.
        /// </summary>
        private readonly IHostEnvironment _env = env;

        /// <summary>
        /// The options for the JSON serializer.
        /// </summary>
        private static readonly JsonSerializerOptions _options =
            new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        /// <summary>
        /// Invokes the exception middleware.
        /// </summary>
        /// <param name="context">
        /// The HTTP context.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                string message = exception.Message;
                _logger.LogError(exception, message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                var response = new ProblemDetails
                {
                    Status = 500,
                    Detail = _env.IsDevelopment() ? exception.StackTrace?.ToString() : null,
                    Title = exception.Message
                };

                var json = JsonSerializer.Serialize(response, _options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
