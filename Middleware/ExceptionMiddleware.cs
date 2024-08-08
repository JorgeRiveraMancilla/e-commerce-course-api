using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_course_api.Middleware
{
    public class ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env
    )
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;
        private readonly IHostEnvironment _env = env;
        private static readonly JsonSerializerOptions _options =
            new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

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
