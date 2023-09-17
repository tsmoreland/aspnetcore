namespace GloboTicket.TicketManagement.Domain.Models.Authentication;

public sealed record class AuthenticationResponse(Guid Id, string UserName, string Email, string Token);
