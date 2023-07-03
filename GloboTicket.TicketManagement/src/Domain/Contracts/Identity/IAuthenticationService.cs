using GloboTicket.TicketManagement.Domain.Models.Authentication;

namespace GloboTicket.TicketManagement.Domain.Contracts.Identity;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
    Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);
}
