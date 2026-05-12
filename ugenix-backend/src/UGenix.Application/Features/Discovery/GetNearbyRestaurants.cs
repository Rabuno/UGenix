using MediatR;
using UGenix.Shared.Abstractions;

namespace UGenix.Application.Features.Discovery;

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
    public Task<Result<PagedList<DiscoveryReadModel>>> Handle(
        GetNearbyRestaurantsQuery request, 
        CancellationToken ct)
    {
        return Task.FromResult(Result<PagedList<DiscoveryReadModel>>.Failure(Error.None));
    }
}

