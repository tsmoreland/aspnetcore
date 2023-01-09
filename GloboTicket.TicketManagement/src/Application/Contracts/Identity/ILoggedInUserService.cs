namespace GloboTicket.TicketManagement.Application.Contracts.Identity;

public interface ILoggedInUserService
{
    public Guid UserId { get; }
}
