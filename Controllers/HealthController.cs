using e_commerce_course_api.Data;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_course_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController(DataContext context, ILogger<HealthController> logger)
    : BaseApiController
{
    private readonly DataContext _context = context;
    private readonly ILogger<HealthController> _logger = logger;

    /// <summary>
    /// Basic health check endpoint
    /// </summary>
    /// <returns>Status indicating if the API is running</returns>
    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Health check requested");

        return Ok(
            new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Service = "E-Commerce Course API",
                Version = "1.0.0",
            }
        );
    }

    /// <summary>
    /// Detailed health check including database connectivity
    /// </summary>
    /// <returns>Detailed status of the application and database</returns>
    [HttpGet("detailed")]
    public async Task<IActionResult> GetDetailed()
    {
        _logger.LogInformation("Detailed health check requested");

        try
        {
            // Test database connectivity
            var canConnect = await _context.Database.CanConnectAsync();

            if (canConnect)
            {
                var healthStatus = new
                {
                    Status = "Healthy",
                    Timestamp = DateTime.UtcNow,
                    Service = "E-Commerce Course API",
                    Version = "1.0.0",
                    Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                        ?? "Development",
                    Database = new { Status = "Connected", ConnectionString = "***hidden***" },
                    System = new
                    {
                        MemoryUsage = GC.GetTotalMemory(false),
                        Environment.ProcessId,
                        Environment.MachineName,
                        OSVersion = Environment.OSVersion.ToString(),
                    },
                };

                return Ok(healthStatus);
            }
            else
            {
                var healthStatus = new
                {
                    Status = "Unhealthy",
                    Timestamp = DateTime.UtcNow,
                    Service = "E-Commerce Course API",
                    Version = "1.0.0",
                    Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                        ?? "Development",
                    Database = new { Status = "Disconnected", ConnectionString = "***hidden***" },
                    System = new
                    {
                        MemoryUsage = GC.GetTotalMemory(false),
                        Environment.ProcessId,
                        Environment.MachineName,
                        OSVersion = Environment.OSVersion.ToString(),
                    },
                };

                return Ok(healthStatus);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during detailed health check");

            var unhealthyStatus = new
            {
                Status = "Unhealthy",
                Timestamp = DateTime.UtcNow,
                Service = "E-Commerce Course API",
                Version = "1.0.0",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                    ?? "Development",
                Database = new
                {
                    Status = "Error",
                    ConnectionString = "***hidden***",
                    Error = ex.Message,
                },
                System = new
                {
                    MemoryUsage = GC.GetTotalMemory(false),
                    Environment.ProcessId,
                    Environment.MachineName,
                    OSVersion = Environment.OSVersion.ToString(),
                },
            };

            return StatusCode(503, unhealthyStatus);
        }
    }

    /// <summary>
    /// Simple ping endpoint for load balancers
    /// </summary>
    /// <returns>Simple OK response</returns>
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("pong");
    }
}
