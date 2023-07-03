namespace GloboTicket.TicketManagement.Domain.Models.Authentication;

public sealed record class AuthenticationRequest(string Email, string Password);
