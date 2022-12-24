
namespace GloboTicket.TicketManagement.Application.Models.Mail;

public sealed record class EmailSettings(string ApiKey, string FromAddress, string FromName);
