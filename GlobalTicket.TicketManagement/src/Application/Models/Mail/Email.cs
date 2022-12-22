namespace GlobalTicket.TicketManagement.Application.Models.Mail;

public sealed record class Email(string To, string Subject, string Body);
