namespace MicroShop.Services.Products.ProductsApiApp.Models;

public sealed class Product
{
    private string _name;
    private string _categoryName;

    public Product(int id, string name, double price, string? description, string categoryName, string? imageUrl, string? imageLocalPath)
    {
        Id = id;
        _name = name;
        _categoryName = categoryName;
        NormalizedName = _name.ToUpperInvariant();
        NormalizedCategoryName = _categoryName.ToUpperInvariant();
        Price = price;
        Description = description;
        ImageUrl = imageUrl;
        ImageLocalPath = imageLocalPath;
    }

    public Product(string name, double price, string? description, string categoryName, string? imageUrl, string? imageLocalPath)
        : this(0, name, price, description, categoryName, imageUrl, imageLocalPath)
    {
        ImageLocalPath = imageLocalPath;
    }

    public int Id { get; set; } 
    public string Name { get => _name; set => SetName(value); } 
    public string NormalizedName { get; private set; }
    public double Price { get; set; } 
    public string? Description { get; set; } 
    public string CategoryName { get => _categoryName; set => SetCategoryName(value); } 
    public string NormalizedCategoryName { get; private set; }
    public string? ImageUrl { get; set; } 
    public string? ImageLocalPath { get; set; }

    private string SetName(string? value)
    {
        _name = value ?? string.Empty;
        NormalizedName = _name.ToUpperInvariant();
        return _name;
    }
    private string SetCategoryName(string? value)
    {
        _categoryName = value ?? string.Empty;
        NormalizedName = _categoryName.ToUpperInvariant();
        return _categoryName;
    }
}
