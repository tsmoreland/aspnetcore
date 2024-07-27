using System.ComponentModel.DataAnnotations;

namespace MicroShop.Services.Products.App.Models.DataTransferObjects;

public sealed record class AddOrEditProductDto
{
    public AddOrEditProductDto(Product product)
        : this(product.Name, product.Price, product.Description, product.CategoryName, product.ImageUrl)
    {
    }

    public AddOrEditProductDto()
        : this(string.Empty, 0, null, string.Empty, null, null)
    {
    }

    // ReSharper disable once ConvertToPrimaryConstructor
    public AddOrEditProductDto(string name, double price, string? description, string categoryName, string? imageUrl, IFormFile? image = null)
    {
        Name = name;
        Price = price;
        Description = description;
        CategoryName = categoryName;
        ImageUrl = imageUrl;
        Image = image;
    }


    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [Required]
    [Range(1, 1000)]
    public double Price { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(200)]
    public string CategoryName { get; set; }

    [MaxLength(200)]
    public string? ImageUrl { get; set; }

    public IFormFile? Image { get; set; }

    // TODO: when updating don't change local path unless image changes
    public Product ToProduct(int id, string imageLocalPath) =>
        new(id, Name, Price, Description, CategoryName, ImageUrl, imageLocalPath);

    public Product ToNewProduct() => new(0, Name, Price, Description, CategoryName, null, null);
}
