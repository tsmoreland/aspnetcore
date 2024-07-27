using System.ComponentModel.DataAnnotations;
using MicroShop.Web.Mvc.App.Validation;

namespace MicroShop.Web.Mvc.App.Models.Products;

public sealed class ProductDto(int id, string name, double price, string? description, string categoryName, string? imageUrl, string? imageLocalPath)
{
    public ProductDto()
        : this(0, string.Empty, 0.0, null, string.Empty, null, null)
    {
    }

    [Required]
    public int Id { get; init; } = id;

    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = name;

    [Required]
    [Range(1, 1000)]
    public double Price { get; init; } = price;

    [MaxLength(500)]
    public string? Description { get; init; } = description;

    [Required]
    [MaxLength(200)]
    public string CategoryName { get; init; } = categoryName;

    [MaxLength(200)]
    public string? ImageUrl { get; init; } = imageUrl;

    [MaxLength(260)]
    public string? ImageLocalPath { get; init; } = imageLocalPath;

    [FileExtensions(Extensions = "jpg,png")]
    [MaxFileSize(1)]
    public IFormFile? Image { get; init; }

    public static ProductDto Empty() => new();
}
