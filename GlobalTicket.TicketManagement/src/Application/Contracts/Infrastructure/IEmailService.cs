using GlobalTicket.TicketManagement.Application.Models.Mail;

namespace GlobalTicket.TicketManagement.Application.Contracts.Infrastructure;

public interface IEmailService
{
    ValueTask<bool> SendEmail(Email email, CancellationToken cancellationToken);
}
