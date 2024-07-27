using System.Diagnostics.CodeAnalysis;

namespace MicroShop.Services.Email.App.Models;

public sealed class EmailLogEntry
{
    private string _emailAddress;

    [SetsRequiredMembers]
    public EmailLogEntry()
        : this(0, string.Empty, string.Empty, null)
    {
    }

    [SetsRequiredMembers]
    public EmailLogEntry(string emailAddress, string message, DateTime? emailSent)
        : this(0, emailAddress, message, emailSent)
    {
    }

    [SetsRequiredMembers]
    private EmailLogEntry(int id, string emailAddress, string message, DateTime? emailSent)
    {
        Id = id;
        EmailAddress = string.Empty; // no-op, this is immediately replaced and only here to suppress a warning
        _emailAddress = emailAddress;
        NormalizedEmailAddress = emailAddress.ToUpperInvariant();
        Message = message;
        EmailSent = emailSent;
    }

    public required int Id { get; init; }
    public required string EmailAddress
    {
        get => _emailAddress;
        init
        {
            _emailAddress = value;
            NormalizedEmailAddress = value.ToUpperInvariant();
        }
    }
    public required string Message { get; init; }
    public DateTime? EmailSent { get; private set; }
    public string NormalizedEmailAddress { get; private set; }


}
