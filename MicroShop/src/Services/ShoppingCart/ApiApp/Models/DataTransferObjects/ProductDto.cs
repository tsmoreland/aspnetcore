using System.ComponentModel.DataAnnotations;

namespace MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects;

public sealed record class ProductDto(
    [property: Required] int Id,
    [property: Required, MaxLength(200)] string Name,
    [property: Required, Range(1, 1000)] double Price,
    [property: MaxLength(500)] string? Description,
    [property: Required, MaxLength(200)] string CategoryName,
    [property: MaxLength(200)] string? ImageUrl,
    [property: MaxLength(260)] string? ImageLocalPath)
{
}
