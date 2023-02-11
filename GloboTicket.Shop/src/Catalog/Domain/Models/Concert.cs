using GloboTicket.Shop.Shared.Contracts.Persistence;
using GloboTicket.Shop.Shared.Models;
using static GloboTicket.Shop.Shared.Validators.ArgumentValidator;

namespace GloboTicket.Shop.Catalog.Domain.Models;

public sealed class Concert : IAuditableEntity
{
    public static readonly int MaxNameLength = 200;
    public static readonly int MaxArtistLength = 200;
    public static readonly int MaxDescriptionLength = 500;

    public Guid ConcertId { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Price { get; private set; }
    public string Artist { get; init; } = string.Empty;
    public DateTime Date { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string? ImageUrl { get; private set; }
    public AuditDetails? Audit { get; private set; }
    public string ConcurrencyToken { get; private set; } = Guid.NewGuid().ToString("N");

    private Concert()
    {
        // used by EF
    }
    private Concert(Guid concertId, string name, int price, string artist, DateTime date, string description, string imageUrl, AuditDetails? audit, string concurrencyToken)
    {
        ConcertId = concertId;
        Name = name;
        Artist = artist;
        Date = date;
        Description = description;
        ImageUrl = imageUrl;
        Audit = audit;
        ConcurrencyToken = concurrencyToken;
    }

    public Concert(string name, string artist, int price, DateTime date, string description)
    {
        ConcertId = Guid.NewGuid();
        Name = RequiresNonEmptyStringWithMaxLength(name, MaxNameLength);
        Price = RequiresGreaterThanOrEqualToZero(price);
        Artist = RequiresNonEmptyStringWithMaxLength(artist, MaxArtistLength);
        Date = RequiresNowOrNewer(date);
        Description = RequiresNonEmptyStringWithMaxLength(description, MaxDescriptionLength);
    }

    public void UpdatePrice(int price)
    {
        Price = RequiresGreaterThanOrEqualToZero(price);
    }

    public void SetImageSource(Uri url)
    {
        ArgumentNullException.ThrowIfNull(url);
        ImageUrl = url.ToString();
    }

    public void ClearImageSource()
    {
        ImageUrl = null;
    }

    public void SetAuditCreatedDetails(string createdBy)
    {
        AuditDetails audit = new(createdBy, DateTime.UtcNow, createdBy, DateTime.UtcNow);
        audit.ThrowIfNotValid(nameof(createdBy));
        Audit = audit;
    }

    public void SetAuditUpdatedDetails(string updatedBy)
    {
        if (Audit is null)
        {
            throw new InvalidOperationException("Audit not set, entity has not yet been created");
        }

        AuditDetails audit = Audit with { LastModifiedBy = updatedBy, LastModifiedDate = DateTime.UtcNow };
        audit.ThrowIfNotValid(nameof(updatedBy));
        Audit = audit;
    }

    public void UpdateConcurrencyToken()
    {
        ConcurrencyToken = Guid.NewGuid().ToString("N");
    }
}
