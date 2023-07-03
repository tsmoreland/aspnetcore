
namespace GloboTicket.TicketManagement.Domain.Models.Mail;

public sealed record class EmailSettings(string ApiKey, string FromAddress, string FromName);
