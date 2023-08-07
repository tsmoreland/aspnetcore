using BethanysPieShop.Admin.Domain.Validation;

namespace BethanysPieShop.Admin.Domain.Models;

public sealed class Pie
{
    private string _name;
    private string? _shortDescription;
    private string? _longDescription;
    private string? _allergyInformation;
    private decimal _price;
    private string? _imageFilename;
    private string? _imageThumbnailFilename;
    private Category? _category;
    private readonly HashSet<Ingredient> _ingredients;

    public Pie(string name, decimal price, string? shortDescription = null, string? longDescription = null, string? allergyInformation = null, string? imageFilename = null, string? imageThumbnailFilename = null, Guid? categoryId = null)
        : this(
              Guid.NewGuid(),
              NameValidator.Instance.ValidateOrThrow(name),
              ShortDescriptionValidator.Instance.ValidateOrThrow(shortDescription),
              LongDescriptionValidator.Instance.ValidateOrThrow(longDescription),
              AllergyInformationValidator.Instance.ValidateOrThrow(allergyInformation),
              price,
              imageFilename,
              imageThumbnailFilename,
              categoryId)
    {
    }

    private Pie(Guid id, string name, string? shortDescription, string? longDescription, string? allergyInformation, decimal price, string? imageFilename, string? imageThumbnailFilename, Guid? categoryId)
    {
        Id = id;
        _name = name;
        _shortDescription = shortDescription;
        _longDescription = longDescription;
        _allergyInformation = allergyInformation;
        _price = price;
        _imageFilename = imageFilename;
        _imageThumbnailFilename = imageThumbnailFilename;
        CategoryId = categoryId;
        _ingredients = new HashSet<Ingredient>();
    }

    public Guid Id { get; }

    public string Name
    {
        get => _name;
        set
        {
            _name = NameValidator.Instance.ValidateOrThrow(value);
        }
    }

    public string? ShortDescription
    {
        get => _shortDescription;
        set
        {
            _shortDescription = ShortDescriptionValidator.Instance.ValidateOrThrow(value);
        }
    }
    public string? LongDescription
    {
        get => _longDescription;
        set
        {
            _longDescription = LongDescriptionValidator.Instance.ValidateOrThrow(value);
        }
    }
    public string? AllergyInformation
    {
        get => _allergyInformation;
        set
        {
            _allergyInformation = AllergyInformationValidator.Instance.ValidateOrThrow(value);
        }
    }
    public decimal Price
    {
        get => _price;
        set
        {
            _price = value;
        }
    }
    public string? ImageFilename
    {
        get => _imageFilename;
        set
        {
            _imageFilename = value;
        }
    }
    public string? ImageThumbnailFilename
    {
        get => _imageThumbnailFilename;
        set
        {
            _imageThumbnailFilename = value;
        }
    }

    public bool IsPieOfTheWeek { get; set; }

    public bool InStock { get; set; }

    public Guid? CategoryId { get; private set; }
    public Category? Category
    {
        get => _category;
        set
        {
            if (value is not null)
            {
                CategoryId = value.CategoryId;
                _category = value;
            }
            else
            {
                CategoryId = null;
                _category = null;
            }
        }
    }

    public ICollection<Ingredient> Ingredients => _ingredients.ToList();
}
