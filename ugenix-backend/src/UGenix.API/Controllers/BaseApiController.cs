using MediatR;
using Microsoft.AspNetCore.Mvc;
using UGenix.API.Abstractions;
using UGenix.Shared.Abstractions;

namespace UGenix.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected readonly ISender Mediator;

    protected BaseApiController(ISender mediator) => Mediator = mediator;

    protected IActionResult HandleFailure(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        return MapFailure(result.Error);
    }

    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return Ok(new ApiEnvelope<T>(result.Value, HttpContext.TraceIdentifier, HttpContext.Items["CorrelationId"]?.ToString() ?? string.Empty));

        return MapFailure(result.Error);
    }

    private IActionResult MapFailure(Error error)
    {
        var status = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        var problemDetails = new ProblemDetails
        {
            Status = status,
            Title = error.Code,
            Detail = error.Description,
            Instance = HttpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = HttpContext.TraceIdentifier;
        problemDetails.Extensions["correlationId"] = HttpContext.Items["CorrelationId"];

        return new ObjectResult(problemDetails) { StatusCode = status };
    }
}

