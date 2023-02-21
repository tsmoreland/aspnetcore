namespace GloboTicket.Shop.Ordering.Application.Models.Mail;

public sealed record class Email(string To, string Subject, string Body);
