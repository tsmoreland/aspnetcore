using MicroShop.Web.MvcApp.Models.Cart;
using MicroShop.Web.MvcApp.Models.Products;

namespace MicroShop.Web.MvcApp.Models;

internal static class Mapper
{
    public static UpsertCartDto ToUpsert(this ProductDto product, int count) =>
        new (null, product.Id, count);
    public static UpsertCartDto ToUpsert(this ProductDto product, int headerId, int count) =>
        new (headerId, product.Id, count);

    public static AddToCartDto ToAddToCart(this ProductDto product, int count) =>
        new(product, count);
    public static UpsertCartDto ToUpsert(this AddToCartDto addToCartItem) =>
        new (null, addToCartItem.Product.Id, addToCartItem.Count);
    public static UpsertCartDto ToUpsert(this AddToCartDto addToCartItem, int headerId) =>
        new (headerId, addToCartItem.Product.Id, addToCartItem.Count);
}
