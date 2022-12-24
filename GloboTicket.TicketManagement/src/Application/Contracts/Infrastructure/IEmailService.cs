using GloboTicket.TicketManagement.Application.Models.Mail;

namespace GloboTicket.TicketManagement.Application.Contracts.Infrastructure;

public interface IEmailService
{
    ValueTask<bool> SendEmail(Email email, CancellationToken cancellationToken);
}
