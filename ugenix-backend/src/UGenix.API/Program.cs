using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Serilog;
using UGenix.API.Extensions;
using UGenix.API.Middleware;
using UGenix.Application;
using UGenix.Infrastructure;
using UGenix.Persistence;
using UGenix.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Serilog Configuration
builder.Host.UseSerilog((context, loggerConfig) => 
    loggerConfig.ReadFrom.Configuration(context.Configuration));

// 1. Core Services Integration
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddPersistence(builder.Configuration)
    .AddApplication();

// Production-specific configuration overrides & validation
if (builder.Environment.IsProduction())
{
    builder.Services.AddProductionConfiguration(builder.Configuration);
}

// 2. API Standard Services
builder.Services.AddControllers();
builder.Services.AddUGenixVersioning();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Security: Rate Limiting (Fixed Window)
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("strict", opt =>
    {
        opt.Window = TimeSpan.FromSeconds(10);
        opt.PermitLimit = 10;
        opt.QueueLimit = 2;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

// 4. Security: CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new[] { "http://localhost:5173" })
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// 5. Middleware Pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("DefaultPolicy");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

// 6. Database Initialization
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<UGenix.Shared.Abstractions.IPasswordHasher>();
    
    // Apply migrations
    await Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions.MigrateAsync(context.Database);
    
    // Seed data
    await DatabaseSeeder.SeedAsync(context, passwordHasher);
}

app.Run();

public partial class Program { }

