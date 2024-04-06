using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects;

namespace MicroShop.Services.ShoppingCart.ApiApp.Models;

public sealed class CartDetails(int id, int headerId, CartHeader header, int productId, ProductDto product)
{
    private ProductDto _productDto = product;

    public CartDetails(CartHeader header, ProductDto product)
        : this(0, header.Id, header, product.Id, product)
    {
    }

    /// <summary>
    /// For EF Core
    /// </summary>
#   pragma warning disable IDE0051 // Remove unused private members
    private CartDetails(int id, int headerId, int productId)
#   pragma warning restore IDE0051 // Remove unused private members
        : this(id, headerId, null!, productId, null!)
    {
    }

    public required int Id { get; init; } = id;
    public required int HeaderId { get; init; } = headerId;

    public required CartHeader Header { get; init; } = header;

    public int ProductId { get; private set; } = productId;

    public ProductDto Product
    {
        get => _productDto;
        init
        {
            _productDto = value;
            ProductId = value.Id;
        }
    }

    public int Count { get; set; }
}
