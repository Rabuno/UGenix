using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using UGem.API.Abstractions;
using UGem.Application.Features.Discovery;
using UGem.Shared.Abstractions;

namespace UGem.API.Controllers;

[ApiVersion("1.0")]
public class DiscoveryController(MediatR.ISender mediator) : BaseApiController(mediator)
{
    /// <summary>
    /// Search for places within a specific radius based on geolocation.
    /// Uses spatial indexing (GIST) for high-performance retrieval.
    /// </summary>
    [HttpGet("nearby")]
    [ProducesResponseType(typeof(ApiEnvelope<PagedList<DiscoveryReadModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNearby(
        [FromQuery] double lat, 
        [FromQuery] double lng, 
        [FromQuery] double radius = 5000, 
        [FromQuery] string? cursor = null, 
        [FromQuery] int limit = 20)
    {
        var query = new GetNearbyRestaurantsQuery(
            lat, 
            lng, 
            radius, 
            new CursorPaginationRequest(cursor != null ? new Cursor(cursor) : null, null, limit));

        var result = await Mediator.Send(query);

        return HandleResult<PagedList<DiscoveryReadModel>>(result);
    }
}
