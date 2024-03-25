using System.ComponentModel.DataAnnotations;

namespace MicroShop.Services.Products.ProductsApiApp.Models.DataTransferObjects;

public sealed class ProductDto(int productId, string name, double price, string? description, string categoryName, string? imageUrl)
{
    public ProductDto()
        : this(0, string.Empty, 0.0, null, string.Empty, null)
    {
    }

    [Required]
    public int ProductId { get; set; } = productId;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = name;

    [Required]
    [Range(1, 1000)]
    public double Price { get; set; } = price;

    [MaxLength(500)]
    public string? Description { get; set; } = description;

    [Required]
    [MaxLength(200)]
    public string CategoryName { get; set; } = categoryName;

    [MaxLength(200)]
    public string? ImageUrl { get; set; } = imageUrl;

}
