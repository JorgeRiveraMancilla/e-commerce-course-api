using System.Text;
using e_commerce_course_api.Data;
using e_commerce_course_api.Data.Repositories;
using e_commerce_course_api.Entities;
using e_commerce_course_api.Interfaces;
using e_commerce_course_api.Middleware;
using e_commerce_course_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "E-Commerce Course API",
            Version = "v1",
            Description = "A comprehensive e-commerce API built with ASP.NET Core",
            Contact = new OpenApiContact
            {
                Name = "E-Commerce Course",
                Email = "contact@ecommerce-course.com",
            },
        }
    );

    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put Bearer + your token in the box below",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme,
        },
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement { { jwtSecurityScheme, Array.Empty<string>() } }
    );
});

// Database configuration with improved error handling
string? connection;

if (builder.Environment.IsDevelopment())
{
    connection =
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("DefaultConnection not found in development");
    Console.WriteLine("Using development connection string");
}
else
{
    Console.WriteLine("Production environment detected, configuring database connection...");

    // First try the configured connection string from Render
    connection = builder.Configuration.GetConnectionString("DefaultConnection");
    Console.WriteLine(
        $"DefaultConnection from config: {(!string.IsNullOrEmpty(connection) ? "Found" : "Empty")}"
    );

    // If empty, try DATABASE_URL environment variable
    if (string.IsNullOrEmpty(connection))
    {
        Console.WriteLine("DefaultConnection is empty, trying DATABASE_URL...");
        var connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        Console.WriteLine(
            $"DATABASE_URL: {(!string.IsNullOrEmpty(connectionUrl) ? "Found" : "Empty")}"
        );

        if (!string.IsNullOrEmpty(connectionUrl))
        {
            Console.WriteLine("Parsing DATABASE_URL...");
            try
            {
                // Parse connection URL to connection string for Npgsql (Render format)
                connectionUrl = connectionUrl.Replace("postgres://", string.Empty);
                var pgUserAndPassword = connectionUrl.Split("@")[0];
                var pgHostPortAndDatabase = connectionUrl.Split("@")[1];
                var pgHostPort = pgHostPortAndDatabase.Split("/")[0];
                var pgDatabase = pgHostPortAndDatabase.Split("/")[1];
                var pgUser = pgUserAndPassword.Split(":")[0];
                var pgPass = pgUserAndPassword.Split(":")[1];
                var pgHost = pgHostPort.Split(":")[0];
                var pgPort = pgHostPort.Split(":")[1];
                var updatedHost = pgHost.Replace("flycast", "internal");

                connection =
                    $"Server={updatedHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDatabase};Ssl Mode=Require;Trust Server Certificate=true;";
                Console.WriteLine("Successfully parsed DATABASE_URL");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing DATABASE_URL: {ex.Message}");
                throw new InvalidOperationException("Failed to parse DATABASE_URL", ex);
            }
        }
    }

    // Final validation
    if (string.IsNullOrEmpty(connection))
    {
        Console.WriteLine("ERROR: No database connection string found");
        throw new InvalidOperationException(
            "No database connection string found. Please check your environment variables."
        );
    }

    // Log connection info (without sensitive data)
    var maskedConnection = connection.Contains("Password=")
        ? connection.Split("Password=")[0]
            + "Password=***"
            + (
                connection.Contains(";")
                    ? ";"
                        + string.Join(
                            ";",
                            connection
                                .Split(";")
                                .Skip(1)
                                .Where(p =>
                                    !p.StartsWith("Password=", StringComparison.OrdinalIgnoreCase)
                                )
                        )
                    : ""
            )
        : connection;
    Console.WriteLine($"Final connection string: {maskedConnection}");
}

builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseNpgsql(connection);
});

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        policy =>
        {
            if (builder.Environment.IsDevelopment())
            {
                policy
                    .WithOrigins(
                        "http://localhost:3000",
                        "http://localhost:5173",
                        "https://localhost:3000",
                        "https://localhost:5173"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            }
            else
            {
                policy
                    .WithOrigins(
                        "https://e-commerce-course-client.netlify.app",
                        "https://your-production-frontend.com"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            }
        }
    );
});

// Identity configuration
builder
    .Services.AddIdentityCore<User>(opt =>
    {
        opt.User.RequireUniqueEmail = true;
        opt.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ";
        opt.Password.RequireDigit = true;
        opt.Password.RequireLowercase = true;
        opt.Password.RequireUppercase = true;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequiredLength = 6;
    })
    .AddRoles<Role>()
    .AddEntityFrameworkStores<DataContext>();

// JWT Authentication
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["JWTSettings:TokenKey"]
                        ?? throw new Exception("Token key not found.")
                )
            ),
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddAuthorization();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Repository and Service registrations
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();

// Add Health Checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Commerce Course API v1");
    });
}
else
{
    // Production Swagger (optional, remove if not needed in production)
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Commerce Course API v1");
        c.RoutePrefix = "docs"; // Access via /docs instead of /swagger
    });
}

// Health check endpoint
app.MapHealthChecks(
    "/health",
    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        ResponseWriter = async (context, report) =>
        {
            context.Response.ContentType = "application/json";

            var result = new
            {
                Status = report.Status.ToString(),
                Timestamp = DateTime.UtcNow,
                Service = "E-Commerce Course API",
                Version = "1.0.0",
                Environment = app.Environment.EnvironmentName,
                Checks = report.Entries.Select(e => new
                {
                    Name = e.Key,
                    Status = e.Value.Status.ToString(),
                    e.Value.Description,
                    Duration = e.Value.Duration.TotalMilliseconds,
                }),
            };

            await context.Response.WriteAsJsonAsync(result);
        },
    }
);

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Database initialization
var scope = app.Services.CreateScope();
var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

try
{
    logger.LogInformation("Starting database migration...");
    await dataContext.Database.MigrateAsync();
    logger.LogInformation("Database migration completed successfully.");

    logger.LogInformation("Starting data seeding...");
    await Seeder.SeedAsync(dataContext, userManager, config);
    logger.LogInformation("Data seeding completed successfully.");
}
catch (Exception exception)
{
    logger.LogError(exception, "An error occurred during application startup.");
    throw; // Re-throw to prevent application from starting in an invalid state
}

// Log application startup
logger.LogInformation("E-Commerce Course API is starting...");
logger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);

app.Run();
