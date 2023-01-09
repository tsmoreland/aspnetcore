namespace GloboTicket.TicketManagement.Application.Models.Authentication;

public sealed record class AuthenticationRequest(string Email, string Password);
