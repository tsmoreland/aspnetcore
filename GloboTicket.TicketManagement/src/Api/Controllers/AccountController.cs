using System.Net.Mime;
using GloboTicket.TicketManagement.Domain.Contracts.Identity;
using GloboTicket.TicketManagement.Domain.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GloboTicket.TicketManagement.Api.Controllers;

[Route("api/account")]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails), ContentTypes = new [] { "application/problem+json" })]
public class AccountController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    /// <inheritdoc />
    public AccountController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
    }

    [HttpPost("authenticate")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(AuthenticationResponse), ContentTypes = new [] { MediaTypeNames.Application.Json })]
    [SwaggerResponse(StatusCodes.Status401Unauthorized,  Type = typeof(ProblemDetails), ContentTypes = new [] { "application/problem+json" })]
    [SwaggerResponse(StatusCodes.Status403Forbidden, Type = typeof(ProblemDetails), ContentTypes = new [] { "application/problem+json" })]
    public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
    {
        return Ok(await _authenticationService.AuthenticateAsync(request));
    }

    [HttpPost("register")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(RegistrationResponse), ContentTypes = new [] { MediaTypeNames.Application.Json })]
    public async Task<IActionResult> RegisterAsync(RegistrationRequest request)
    {
        return Ok(await _authenticationService.RegisterAsync(request));
    }

}
