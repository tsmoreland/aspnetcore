using System.ComponentModel.DataAnnotations;

namespace MicroShop.Web.MvcApp.Models.Cart;

public sealed class AddToCartDto(int productId, string productName, string? productDescription, string catagoryName, double productPrice, string? productImageUrl, int count = 1)
{
    public AddToCartDto()
        : this(0, string.Empty, null,string.Empty, 0.0, null, 0)
    {
    }


    public int ProductId { get; init; } = productId;
    public string ProductName { get; init; } = productName;
    public string? ProductDescription { get; init; } = productDescription;
    public string CatagoryName { get; init;  } = catagoryName;
    public double ProductPrice { get; init; } = productPrice;
    public string? ProductImageUrl { get; init; } = productImageUrl;

    [Range(1, 999)]
    public int Count { get; set; } = count;
}
