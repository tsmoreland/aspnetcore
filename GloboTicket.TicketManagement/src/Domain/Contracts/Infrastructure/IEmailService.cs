using GloboTicket.TicketManagement.Domain.Models.Mail;

namespace GloboTicket.TicketManagement.Domain.Contracts.Infrastructure;

public interface IEmailService
{
    ValueTask<bool> SendEmail(Email email, CancellationToken cancellationToken);
}
