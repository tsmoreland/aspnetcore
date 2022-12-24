namespace GloboTicket.TicketManagement.UI.BlazorWasm.App.Contracts;

public interface IAuthenticationService
{
    ValueTask<bool> Authenticate(string email, string password);
    ValueTask<bool> Register(string firstName, string lastName, string userName, string email, string password);
    ValueTask Logout();
}
