using System.ComponentModel.DataAnnotations;

namespace MicroShop.Services.Products.ProductsApiApp.Models.DataTransferObjects;

public sealed record class AddOrEditProductDto(
    [property: Required, MaxLength(200)] string Name,
    [property: Required, Range(1, 1000)] double Price,
    [property: MaxLength(500)] string? Description,
    [property: Required, MaxLength(200)] string CategoryName,
    [property: MaxLength(200)] string? ImageUrl,
    [property: MaxLength(260)] string? ImageLocalPath,
    IFormFile? Image = null)
{
    public AddOrEditProductDto(Product product)
        : this(product.Name, product.Price, product.Description, product.CategoryName, product.ImageUrl, product.ImageLocalPath)
    {
    }

    public Product ToProduct(int id) => new(id, Name, Price, Description, CategoryName, ImageUrl, ImageLocalPath);

    public Product ToNewProduct() => new(0, Name, Price, Description, CategoryName, ImageUrl, ImageLocalPath);
}
