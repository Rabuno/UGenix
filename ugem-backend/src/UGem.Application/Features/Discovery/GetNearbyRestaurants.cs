using MediatR;
using UGem.Shared.Abstractions;

namespace UGem.Application.Features.Discovery;

public record DiscoveryReadModel(
    RestaurantId Id,
    string Name,
    string Description,
    double Latitude,
    double Longitude,
    double DistanceMeters,
    double AverageRating,
    int ReviewCount,
    bool IsTrending);

public record GetNearbyRestaurantsQuery(
    double Latitude,
    double Longitude,
    double RadiusMeters,
    CursorPaginationRequest Pagination) : IRequest<Result<PagedList<DiscoveryReadModel>>>;

public class GetNearbyRestaurantsHandler 
    : IRequestHandler<GetNearbyRestaurantsQuery, Result<PagedList<DiscoveryReadModel>>>
{
    // Implementation uses IReadDbContext and WithinSafeDistance spatial extensions
    public Task<Result<PagedList<DiscoveryReadModel>>> Handle(
        GetNearbyRestaurantsQuery request, 
        CancellationToken ct)
    {
        // 1. Validate Radius
        // 2. Query ReadModel with Spatial Filter
        // 3. Apply Ranking (Trending > Rating > Distance)
        // 4. Return Cursor-paginated result
        return Task.FromResult(Result<PagedList<DiscoveryReadModel>>.Failure(Error.None));
    }
}
