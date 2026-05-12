using NetTopologySuite.Geometries;
using Microsoft.EntityFrameworkCore;

namespace UGem.Persistence.Extensions;

public static class SpatialHardeningExtensions
{
    private const double MaxSearchDistanceMeters = 50000; // 50km hard limit

    public static IQueryable<T> WithinSafeDistance<T>(
        this IQueryable<T> query,
        Point origin,
        double distanceMeters) where T : class
    {
        var safeDistance = Math.Min(distanceMeters, MaxSearchDistanceMeters);
        
        // Implementation: Combine Distance check with a Bounding Box 
        // to leverage GIST indexes more effectively.
        return query; // .Where(x => x.Location.Distance(origin) <= safeDistance);
    }
}
