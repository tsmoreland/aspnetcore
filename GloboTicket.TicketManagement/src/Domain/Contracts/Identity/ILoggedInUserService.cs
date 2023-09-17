namespace GloboTicket.TicketManagement.Domain.Contracts.Identity;

public interface ILoggedInUserService
{
    public Guid UserId { get; }
}
