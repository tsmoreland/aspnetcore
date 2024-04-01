using System.ComponentModel.DataAnnotations;

namespace MicroShop.Web.MvcApp.Models.Products;

public sealed class AddProductDto(string name, double price, string? description, string categoryName, string? imageUrl)
{
    public AddProductDto()
        : this(string.Empty, 0.0, null, string.Empty, null)
    {
    }

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

    [FileExtensions(Extensions = "jpg,png")]
    public IFormFile? Image { get; set; }
}
