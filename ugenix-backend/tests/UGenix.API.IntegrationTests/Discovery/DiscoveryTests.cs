using System.Net.Http.Json;
using FluentAssertions;
using UGenix.Application.Features.Discovery;
using UGenix.Shared.Abstractions;
using UGenix.API.Abstractions;
using Xunit;

namespace UGenix.API.IntegrationTests.Discovery;

public class DiscoveryTests : BaseIntegrationTest
{
    [Fact]
    public async Task GetNearby_ShouldReturnOk_WithValidCoordinates()
    {
        // Arrange
        var lat = 10.762622;
        var lng = 106.660172;
        var radius = 5000;

        // Act
        var response = await _client.GetAsync($"/api/v1/discovery/nearby?lat={lat}&lng={lng}&radius={radius}");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ApiEnvelope<PagedList<DiscoveryReadModel>>>();
        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
    }
}
