using NetTopologySuite.Geometries;
using UGenix.Domain.Abstractions;

namespace UGenix.Domain.Entities;

public class Restaurant : BaseEntity, IAggregateRoot
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    
    // PostGIS Geography Point (SRID 4326)
    public Point Location { get; set; } = default!;
    
    public string? ThumbnailUrl { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
    
    public Guid MerchantId { get; set; }
}

