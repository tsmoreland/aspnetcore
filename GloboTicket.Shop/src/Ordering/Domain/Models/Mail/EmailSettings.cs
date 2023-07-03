
namespace GloboTicket.Shop.Ordering.Domain.Models.Mail;

public sealed class EmailSettings
{
    public string ApiKey { get; init; } = string.Empty;
    public string FromAddress { get; init; } = string.Empty;
    public string FromName { get; init; } = string.Empty;
}
