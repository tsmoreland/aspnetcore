using System.Diagnostics.CodeAnalysis;
using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects;

namespace MicroShop.Services.ShoppingCart.ApiApp.Models;

public sealed class CartDetails(int id, int headerId, CartHeader header, int productId, ProductDto product)
{
    private ProductDto _productDto = product;

    [SetsRequiredMembers]
    public CartDetails(CartHeader header, ProductDto product)
        : this(0, header.Id, header, product.Id, product)
    {
    }

    /// <summary>
    /// For EF Core
    /// </summary>
    [SetsRequiredMembers]
    internal CartDetails(int id, int headerId, int productId)
        : this(id, headerId, null!, productId, null!)
    {
    }
    /// <summary>
    /// For EF Core
    /// </summary>
    [SetsRequiredMembers]
    internal CartDetails(int headerId, int productId)
        : this(0, headerId, productId)
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
