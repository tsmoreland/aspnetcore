using System.ComponentModel.DataAnnotations;

namespace MicroShop.Services.Products.App.Models.DataTransferObjects;

public sealed record class ProductDto(
    [property: Required] int Id,
    [property: Required, MaxLength(200)] string Name,
    [property: Required, Range(1, 1000)] double Price,
    [property: MaxLength(500)] string? Description,
    [property: Required, MaxLength(200)] string CategoryName,
    [property: MaxLength(200)] string? ImageUrl,
    [property: MaxLength(260)] string? ImageLocalPath)
{
    public ProductDto(Product product)
        : this(product.Id, product.Name, product.Price, product.Description, product.CategoryName, product.ImageUrl, product.ImageLocalPath)
    {
    }

    public Product ToProduct() => new(Id, Name, Price, Description, CategoryName, ImageUrl, ImageLocalPath);
}
