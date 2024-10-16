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
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement { { jwtSecurityScheme, Array.Empty<string>() } }
    );
});
string connection;
if (builder.Environment.IsDevelopment())
    connection = builder.Configuration.GetConnectionString("DefaultConnection")!;
else
{
    // Use connection string provided at runtime by FlyIO.
    var connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL")!;

    // Parse connection URL to connection string for Npgsql
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
        $"Server={updatedHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDatabase};";
}
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseNpgsql(connection);
});
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins(
                    "http://localhost:5173",
                    "https://e-commerce-course-web-client.vercel.app"
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    );
});
builder
    .Services.AddIdentityCore<User>(opt =>
    {
        opt.User.RequireUniqueEmail = true;
        opt.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ";
    })
    .AddRoles<Role>()
    .AddEntityFrameworkStores<DataContext>();
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
            )
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
    });
}
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

var scope = app.Services.CreateScope();
var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
try
{
    await dataContext.Database.MigrateAsync();
    await Seeder.SeedAsync(dataContext, userManager);
}
catch (Exception exception)
{
    logger.LogError(exception, "An error occurred during migration.");
}

app.Run();
