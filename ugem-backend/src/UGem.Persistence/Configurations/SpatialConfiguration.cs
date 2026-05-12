using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace UGem.Persistence.Configurations;

public static class SpatialMappingExtensions
{
    public static void ConfigureSpatial(this ModelBuilder modelBuilder)
    {
        // Global configuration for PostGIS points
        // Use SRID 4326 (WGS 84) for Geography
    }
}

public record Location(double Latitude, double Longitude)
{
    public Point ToPoint() => new Point(Longitude, Latitude) { SRID = 4326 };
}
