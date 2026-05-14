# Backend Testing & Production Hardening Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Establish a robust testing suite (Unit + Integration) and apply production-grade hardening measures to the backend.

**Architecture:** We will use xUnit for testing, NSubstitute for mocking, and Testcontainers for real DB/Redis integration. Hardening involves Serilog structured logging, Health Checks, and Security Header middleware.

**Tech Stack:** .NET 9, xUnit, FluentAssertions, NSubstitute, Testcontainers, Serilog, Microsoft HealthChecks.

---

### Task 1: Initialize Test Projects & Unit Test Auth Logic

**Files:**
- Create: `ugenix-backend/tests/UGenix.Application.UnitTests/UGenix.Application.UnitTests.csproj`
- Create: `ugenix-backend/tests/UGenix.Application.UnitTests/Identity/IdentityServiceTests.cs`
- Modify: `ugenix-backend/UGenix.sln`

- [ ] **Step 1: Create Unit Test Project**

```bash
cd ugenix-backend
mkdir tests/UGenix.Application.UnitTests
dotnet new xunit -n UGenix.Application.UnitTests -o tests/UGenix.Application.UnitTests
dotnet sln UGenix.sln add tests/UGenix.Application.UnitTests/UGenix.Application.UnitTests.csproj
dotnet add tests/UGenix.Application.UnitTests/UGenix.Application.UnitTests.csproj reference src/UGenix.Application/UGenix.Application.csproj
dotnet add tests/UGenix.Application.UnitTests/UGenix.Application.UnitTests.csproj package NSubstitute
dotnet add tests/UGenix.Application.UnitTests/UGenix.Application.UnitTests.csproj package FluentAssertions
```

- [ ] **Step 2: Write Unit Test for IdentityService**

Create `ugenix-backend/tests/UGenix.Application.UnitTests/Identity/IdentityServiceTests.cs`:

```csharp
using FluentAssertions;
using NSubstitute;
using UGenix.Application.Abstractions;
using UGenix.Infrastructure.Security;
using UGenix.Shared.Abstractions;
using Xunit;

namespace UGenix.Application.UnitTests.Identity;

public class IdentityServiceTests
{
    private readonly IJwtService _jwtService = Substitute.For<IJwtService>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly IRepository<UGenix.Domain.Entities.User> _userRepository = Substitute.For<IRepository<UGenix.Domain.Entities.User>>();
    private readonly IdentityService _sut;

    public IdentityServiceTests()
    {
        _sut = new IdentityService(_userRepository, _passwordHasher, _jwtService);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnFailure_WhenUserDoesNotExist()
    {
        // Arrange
        _userRepository.GetByEmailAsync(Arg.Any<string>()).Returns((UGenix.Domain.Entities.User?)null);

        // Act
        var result = await _sut.LoginAsync("nonexistent@test.com", "password");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Identity.InvalidCredentials");
    }
}
```

- [ ] **Step 3: Run Unit Tests**

Run: `dotnet test tests/UGenix.Application.UnitTests`
Expected: PASS

- [ ] **Step 4: Commit**

```bash
git add ugenix-backend/UGenix.sln ugenix-backend/tests/UGenix.Application.UnitTests
git commit -m "test(backend): setup unit testing and add identity tests"
```

### Task 2: Setup Integration Testing Infrastructure (Testcontainers)

**Files:**
- Create: `ugenix-backend/tests/UGenix.API.IntegrationTests/UGenix.API.IntegrationTests.csproj`
- Create: `ugenix-backend/tests/UGenix.API.IntegrationTests/BaseIntegrationTest.cs`

- [ ] **Step 1: Create Integration Test Project**

```bash
cd ugenix-backend
mkdir tests/UGenix.API.IntegrationTests
dotnet new xunit -n UGenix.API.IntegrationTests -o tests/UGenix.API.IntegrationTests
dotnet sln UGenix.sln add tests/UGenix.API.IntegrationTests/UGenix.API.IntegrationTests.csproj
dotnet add tests/UGenix.API.IntegrationTests/UGenix.API.IntegrationTests.csproj reference src/UGenix.API/UGenix.API.csproj
dotnet add tests/UGenix.API.IntegrationTests/UGenix.API.IntegrationTests.csproj package Microsoft.AspNetCore.Mvc.Testing
dotnet add tests/UGenix.API.IntegrationTests/UGenix.API.IntegrationTests.csproj package Testcontainers.PostgreSql
dotnet add tests/UGenix.API.IntegrationTests/UGenix.API.IntegrationTests.csproj package Testcontainers.Redis
dotnet add tests/UGenix.API.IntegrationTests/UGenix.API.IntegrationTests.csproj package FluentAssertions
```

- [ ] **Step 2: Create Base Integration Test Class**

Create `ugenix-backend/tests/UGenix.API.IntegrationTests/BaseIntegrationTest.cs`:

```csharp
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;
using Xunit;
using UGenix.Persistence;
using Microsoft.EntityFrameworkCore;

namespace UGenix.API.IntegrationTests;

public abstract class BaseIntegrationTest : IAsyncLifetime
{
    protected readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgis/postgis:15-3.3")
        .Build();

    protected readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:7.0-alpine")
        .Build();

    protected HttpClient _client = null!;
    protected IServiceScope _scope = null!;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _redisContainer.StartAsync();

        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql(_dbContainer.GetConnectionString(), o => o.UseNetTopologySuite()));
                });
            });

        _client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
    }

    public async Task DisposeAsync()
    {
        _scope.Dispose();
        await _dbContainer.StopAsync();
        await _redisContainer.StopAsync();
    }
}
```

- [ ] **Step 3: Commit**

```bash
git add ugenix-backend/UGenix.sln ugenix-backend/tests/UGenix.API.IntegrationTests
git commit -m "test(backend): setup integration testing infrastructure with testcontainers"
```

### Task 3: Production Hardening - Health Checks & Security

**Files:**
- Modify: `ugenix-backend/src/UGenix.API/Program.cs`
- Modify: `ugenix-backend/src/UGenix.Infrastructure/DependencyInjection.cs`

- [ ] **Step 1: Add Health Checks Configuration**

Modify `ugenix-backend/src/UGenix.Infrastructure/DependencyInjection.cs` to add health checks:

```csharp
// ugenix-backend/src/UGenix.Infrastructure/DependencyInjection.cs
// Add to AddInfrastructure method:
services.AddHealthChecks()
    .AddNpgSql(configuration.GetConnectionString("Database")!)
    .AddRedis(configuration.GetConnectionString("Redis")!);
```

- [ ] **Step 2: Map Health Checks Endpoint**

Modify `ugenix-backend/src/UGenix.API/Program.cs`:

```csharp
// ugenix-backend/src/UGenix.API/Program.cs
// Add before app.Run():
app.MapHealthChecks("/health");
```

- [ ] **Step 3: Commit**

```bash
git add ugenix-backend/src/UGenix.Infrastructure/DependencyInjection.cs ugenix-backend/src/UGenix.API/Program.cs
git commit -m "perf(backend): add health checks for database and redis"
```

### Task 4: Finalize Serilog & Seq Integration

**Files:**
- Modify: `ugenix-backend/src/UGenix.API/Program.cs`
- Modify: `ugenix-backend/src/UGenix.API/appsettings.json`

- [ ] **Step 1: Configure Serilog in Program.cs**

```csharp
// ugenix-backend/src/UGenix.API/Program.cs
// Add at the very beginning:
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());
```

- [ ] **Step 2: Run and Verify**

Run: `dotnet build ugenix-backend`
Expected: SUCCESS

- [ ] **Step 3: Commit**

```bash
git add ugenix-backend/src/UGenix.API/Program.cs ugenix-backend/src/UGenix.API/appsettings.json
git commit -m "perf(backend): finalize serilog and seq integration"
```
