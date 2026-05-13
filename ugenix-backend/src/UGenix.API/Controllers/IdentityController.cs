using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UGenix.Application.Abstractions;

namespace UGenix.API.Controllers;

[ApiVersion("1.0")]
[AllowAnonymous]
public class IdentityController(ISender mediator, IIdentityService identityService) : BaseApiController(mediator)
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await identityService.LoginAsync(request.Email, request.Password);
        return HandleResult(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await identityService.RegisterAsync(request.Email, request.Password);
        return HandleResult(result);
    }
}

public record LoginRequest(string Email, string Password);
public record RegisterRequest(string Email, string Password);
