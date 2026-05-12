using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UGenix.API.Abstractions;
using UGenix.Application.Features.Reviews;
using UGenix.Shared.Abstractions;

namespace UGenix.API.Controllers;

[ApiVersion("1.0")]
public class ReviewController(MediatR.ISender mediator) : BaseApiController(mediator)
{
    /// <summary>
    /// Submit a new review for a restaurant.
    /// Requires authentication and includes anti-fraud metadata capture.
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ApiEnvelope<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Submit([FromBody] SubmitReviewRequest request)
    {
        var command = new SubmitReviewCommand(
            request.RestaurantId,
            request.Rating,
            request.Comment,
            HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            Request.Headers.UserAgent.ToString()
        );

        var result = await Mediator.Send(command);

        return HandleResult<Guid>(result);
    }
}

public record SubmitReviewRequest(Guid RestaurantId, int Rating, string Comment);

