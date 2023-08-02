using BethanysPieShop.Admin.Domain.Validation;

namespace BethanysPieShop.Admin.Domain.Models;

public sealed class Category
{
    private string _name;
    private string? _description;

    public Category(string name, string? description)
    {
        CategoryId = Guid.NewGuid();
        _name = name;
        _description = description;
    }

    /// <summary>
    /// Category Id
    /// </summary>
    public Guid CategoryId { get; }

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
}
