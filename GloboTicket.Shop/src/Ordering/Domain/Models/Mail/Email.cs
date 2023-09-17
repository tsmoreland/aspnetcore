namespace GloboTicket.Shop.Ordering.Domain.Models.Mail;

public sealed record class Email(string To, string Subject, string Body);
