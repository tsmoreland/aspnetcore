using System.ComponentModel.DataAnnotations;
using MicroShop.Web.MvcApp.Models.Products;

namespace MicroShop.Web.MvcApp.Models.Cart;

public sealed class AddToCartDto(ProductDto product, int count = 1)
{
    public ProductDto Product { get; init; } = product;

    [Range(1, 999)]
    public int Count { get; set; } = count;
}
