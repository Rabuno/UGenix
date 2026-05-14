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
