# Refactor IdentityService and Implement Unit Tests Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Refactor `IdentityService` to use `IRepository<User>` and `IUnitOfWork`, and implement unit tests to verify the `LoginAsync` failure case.

**Architecture:** Use Repository and Unit of Work patterns to decouple `IdentityService` from `ApplicationDbContext`, enabling easier unit testing with mocks.

**Tech Stack:** .NET 8, EF Core, xUnit, NSubstitute, FluentAssertions.

---

### Task 1: Update IRepository and Repository

**Files:**
- Modify: `ugenix-backend/src/UGenix.Shared/Abstractions/IRepository.cs`
- Modify: `ugenix-backend/src/UGenix.Persistence/Repository.cs`

- [ ] **Step 1: Add GetByEmailAsync and AnyAsync to IRepository<T>**

```csharp
using System.Linq.Expressions;

namespace UGenix.Shared.Abstractions;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<T?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task AddAsync(T entity, CancellationToken ct = default);
    void Update(T entity);
    void Delete(T entity);
}
```

- [ ] **Step 2: Implement new methods in Repository<T>**

```csharp
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UGenix.Shared.Abstractions;

namespace UGenix.Persistence;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _dbContext;

    public Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbContext.Set<T>().FindAsync(new object[] { id }, ct);
    }

    public async Task<T?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return await _dbContext.Set<T>()
            .FirstOrDefaultAsync(e => EF.Property<string>(e, "Email") == email, ct);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        return await _dbContext.Set<T>().AnyAsync(predicate, ct);
    }

    public async Task AddAsync(T entity, CancellationToken ct = default)
    {
        await _dbContext.Set<T>().AddAsync(entity, ct);
    }

    public void Update(T entity)
    {
        _dbContext.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }
}
```

- [ ] **Step 3: Verify compilation**

Run: `dotnet build ugenix-backend/UGenix.sln`
Expected: PASS

- [ ] **Step 4: Commit**

```bash
git add ugenix-backend/src/UGenix.Shared/Abstractions/IRepository.cs ugenix-backend/src/UGenix.Persistence/Repository.cs
git commit -m "feat: add GetByEmailAsync and AnyAsync to IRepository"
```

---

### Task 2: Refactor IdentityService

**Files:**
- Modify: `ugenix-backend/src/UGenix.Infrastructure/Security/IdentityService.cs`

- [ ] **Step 1: Refactor IdentityService to use IRepository and IUnitOfWork**

```csharp
using UGenix.Application.Abstractions;
using UGenix.Domain.Entities;
using UGenix.Shared.Abstractions;
using UGenix.Shared.Constants;
using UGenix.Domain.Primitives;

namespace UGenix.Infrastructure.Security;

public class IdentityService(
    IRepository<User> userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IJwtService jwtService) : IIdentityService
{
    public async Task<Result<AuthResponse>> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        var user = await userRepository.GetByEmailAsync(email, ct);

        if (user is null || !passwordHasher.VerifyPassword(password, user.PasswordHash))
        {
            return Result<AuthResponse>.Failure(new Error("Identity.InvalidCredentials", "Invalid email or password.", ErrorType.Unauthorized));
        }

        var token = jwtService.GenerateAccessToken(new UserId(user.Id), user.Email, new[] { user.Role });
        
        return Result<AuthResponse>.Success(new AuthResponse(
            token,
            new UserDto(user.Id, user.Email, user.Role)));
    }

    public async Task<Result<AuthResponse>> RegisterAsync(string email, string password, CancellationToken ct = default)
    {
        if (await userRepository.AnyAsync(u => u.Email == email, ct))
        {
            return Result<AuthResponse>.Failure(new Error("Identity.EmailAlreadyExists", "Email is already in use.", ErrorType.Conflict));
        }

        var user = User.Create(
            email,
            passwordHasher.HashPassword(password),
            Roles.Customer);

        await userRepository.AddAsync(user, ct);
        await unitOfWork.SaveChangesAsync(ct);

        var token = jwtService.GenerateAccessToken(new UserId(user.Id), user.Email, new[] { user.Role });

        return Result<AuthResponse>.Success(new AuthResponse(
            token,
            new UserDto(user.Id, user.Email, user.Role)));
    }
}
```

- [ ] **Step 2: Update Dependency Injection in Infrastructure**

**Files:**
- Modify: `ugenix-backend/src/UGenix.Infrastructure/DependencyInjection.cs`

```csharp
// Ensure IdentityService is registered with new constructor dependencies
// It's likely already registered as services.AddScoped<IIdentityService, IdentityService>();
// Since IRepository<> and IUnitOfWork are already registered in Persistence, it should work.
```

I need to read `ugenix-backend/src/UGenix.Infrastructure/DependencyInjection.cs` first to be sure.

- [ ] **Step 3: Verify compilation**

Run: `dotnet build ugenix-backend/UGenix.sln`
Expected: PASS

- [ ] **Step 4: Commit**

```bash
git add ugenix-backend/src/UGenix.Infrastructure/Security/IdentityService.cs
git commit -m "refactor: IdentityService to use IRepository and IUnitOfWork"
```

---

### Task 3: Setup Unit Test Project and Implement Test

**Files:**
- Modify: `ugenix-backend/tests/UGenix.Application.UnitTests/UGenix.Application.UnitTests.csproj`
- Create: `ugenix-backend/tests/UGenix.Application.UnitTests/Identity/IdentityServiceTests.cs`

- [ ] **Step 1: Add Project Reference to UGenix.Infrastructure in UnitTests project**

```bash
dotnet add ugenix-backend/tests/UGenix.Application.UnitTests/UGenix.Application.UnitTests.csproj reference ugenix-backend/src/UGenix.Infrastructure/UGenix.Infrastructure.csproj
```

- [ ] **Step 2: Create IdentityServiceTests.cs**

```csharp
using FluentAssertions;
using NSubstitute;
using UGenix.Application.Abstractions;
using UGenix.Infrastructure.Security;
using UGenix.Shared.Abstractions;
using UGenix.Domain.Entities;
using Xunit;

namespace UGenix.Application.UnitTests.Identity;

public class IdentityServiceTests
{
    private readonly IJwtService _jwtService = Substitute.For<IJwtService>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly IRepository<User> _userRepository = Substitute.For<IRepository<User>>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IdentityService _sut;

    public IdentityServiceTests()
    {
        _sut = new IdentityService(_userRepository, _unitOfWork, _passwordHasher, _jwtService);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnFailure_WhenUserDoesNotExist()
    {
        // Arrange
        _userRepository.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((User?)null);

        // Act
        var result = await _sut.LoginAsync("nonexistent@test.com", "password");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Identity.InvalidCredentials");
    }
}
```

- [ ] **Step 3: Run the test**

Run: `dotnet test ugenix-backend/tests/UGenix.Application.UnitTests/UGenix.Application.UnitTests.csproj --filter IdentityServiceTests`
Expected: PASS

- [ ] **Step 4: Commit**

```bash
git add ugenix-backend/tests/UGenix.Application.UnitTests/
git commit -m "test: add unit test for IdentityService.LoginAsync failure"
```
