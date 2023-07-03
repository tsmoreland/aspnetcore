using GloboTicket.TicketManagement.Domain.Contracts.Infrastructure;
using GloboTicket.TicketManagement.Domain.Models.Mail;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace GloboTicket.TicketManagement.Infrastructure.Mail;

public sealed class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> mailSettings)
    {
        ArgumentNullException.ThrowIfNull(mailSettings);
        _emailSettings = mailSettings.Value;
    }

    /// <inheritdoc />
    public async ValueTask<bool> SendEmail(Email email, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(email);
        SendGridClient client = new(_emailSettings.ApiKey);
        (string to, string subject, string body) = email;

        EmailAddress from = new() { Email = _emailSettings.FromAddress, Name = _emailSettings.FromName };

        SendGridMessage message = MailHelper.CreateSingleEmail(from, new EmailAddress(to), subject, body, body);
        Response response = await client.SendEmailAsync(message, cancellationToken);

        return response.WasSingleEmailSuccessful();
    }
}
