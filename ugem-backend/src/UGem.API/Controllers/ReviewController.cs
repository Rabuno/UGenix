using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UGem.Application.Features.Reviews;
using UGem.Shared.Abstractions;

namespace UGem.Api.Controllers;

[ApiVersion("1.0")]
public class ReviewController : BaseApiController
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

        return result.IsSuccess 
            ? Ok(result.Value) 
            : HandleFailure(result);
    }
}

public record SubmitReviewRequest(Guid RestaurantId, int Rating, string Comment);
