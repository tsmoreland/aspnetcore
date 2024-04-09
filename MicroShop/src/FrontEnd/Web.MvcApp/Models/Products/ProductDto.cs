using System.ComponentModel.DataAnnotations;

namespace MicroShop.Web.MvcApp.Models.Products;

public sealed class ProductDto(int id, string name, double price, string? description, string categoryName, string? imageUrl, string? imageLocalPath)
{
    public ProductDto()
        : this(0, string.Empty, 0.0, null, string.Empty, null, null)
    {
    }

    [Required]
    public int Id { get; set; } = id;

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

    [MaxLength(260)]
    public string? ImageLocalPath { get; set; } = imageLocalPath;

    [FileExtensions(Extensions = "jpg,png")]
    public IFormFile? Image { get; set; }

    public static ProductDto Empty() => new();
}
