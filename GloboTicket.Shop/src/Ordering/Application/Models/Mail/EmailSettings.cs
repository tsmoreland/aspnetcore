
namespace GloboTicket.Shop.Ordering.Application.Models.Mail;

public sealed class EmailSettings
{
    public string ApiKey { get; init; } = string.Empty;
    public string FromAddress { get; init; } = string.Empty;
    public string FromName { get; init; } = string.Empty;
}
