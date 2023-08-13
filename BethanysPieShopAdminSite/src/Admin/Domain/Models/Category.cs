using BethanysPieShop.Admin.Domain.Validation;

namespace BethanysPieShop.Admin.Domain.Models;

public sealed class Category
{
    private string _name;
    private string? _description;
    private readonly HashSet<Pie> _pies;


    public Category(string name, string? description)
        : this(Guid.NewGuid(), NameValidator.Instance.ValidateOrThrow(name), DescriptionValidator.Instance.ValidateOrThrow(description), DateTime.UtcNow)
    {
    }

    private Category(Guid id, string name, string? description, DateTime? dateAdded)
    {
        Id = id;
        _name = name;
        _description = description;
        DateAdded = dateAdded;
        _pies = new HashSet<Pie>();
    }

    /// <summary>
    /// Category Id
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Category Name
    /// </summary>
    public string Name
    {
        get => _name;
        set => _name = NameValidator.Instance.GetValidatedValueOrThrow(value);
    }

    /// <summary>
    /// Optional Category Description
    /// </summary>
    public string? Description
    {
        get => _description;
        set => _description = DescriptionValidator.Instance.GetValidatedValueOrThrow(value);
    }

    public DateTime? DateAdded { get; init; }

    public ICollection<Pie> Pies => _pies.ToList();

    public byte[] ConcurrencyToken { get; set; } = Array.Empty<byte>();

    public void AddPie(Pie pie)
    {
        ArgumentNullException.ThrowIfNull(pie);
        _pies.Add(pie);
    }
}
