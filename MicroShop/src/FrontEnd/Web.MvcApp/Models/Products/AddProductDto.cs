using System.ComponentModel.DataAnnotations;
using MicroShop.Web.MvcApp.Validation;

namespace MicroShop.Web.MvcApp.Models.Products;

public sealed class AddProductDto(string name, double price, string? description, string categoryName, string? imageUrl)
{
    public AddProductDto()
        : this(string.Empty, 0.0, null, string.Empty, null)
    {
    }

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

    [FileExtensions(Extensions = "jpg,png")]
    [MaxFileSize(1)]
    public IFormFile? Image { get; init; }
}
