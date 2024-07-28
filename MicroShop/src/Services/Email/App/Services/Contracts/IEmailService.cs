using MicroShop.Services.Email.App.Models;

namespace MicroShop.Services.Email.App.Services.Contracts;

public interface IEmailService
{
    ValueTask LogAsync(EmailLogEntry logEntry, CancellationToken cancellationToken = default);
    void Log(EmailLogEntry logEntry);
}
