namespace MicroShop.Services.Products.ProductsApiApp.Models;

public sealed class Product(int productId, string name, double price, string? description, string categoryName, string? imageUrl)
{
    public Product(string name, double price, string? description, string categoryName, string? imageUrl)
        : this(0, name, price, description, categoryName, imageUrl)
    {
    }

    public int ProductId { get; set; } = productId;
    public string Name { get; set; } = name;
    public double Price { get; set; } = price;
    public string? Description { get; set; } = description;
    public string CategoryName { get; set; } = categoryName;
    public string? ImageUrl { get; set; } = imageUrl;
   
}
