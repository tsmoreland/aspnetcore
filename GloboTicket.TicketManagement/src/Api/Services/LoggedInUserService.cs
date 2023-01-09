
using System.Security.Claims;
using GloboTicket.TicketManagement.Application.Contracts.Identity;

namespace GloboTicket.TicketManagement.Api.Services;

public sealed class LoggedInUserService : ILoggedInUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoggedInUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    /// <inheritdoc />
    public Guid UserId => GetUserIdFromHttpContextOrEmpty();

    private Guid GetUserIdFromHttpContextOrEmpty()
    {
        string? id = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(id, out Guid userId)
            ? userId
            : Guid.Empty;
    }
}
