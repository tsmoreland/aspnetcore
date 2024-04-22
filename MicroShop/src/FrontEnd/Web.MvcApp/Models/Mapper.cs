using System.Runtime.CompilerServices;
using MicroShop.Web.MvcApp.Models.Cart;
using MicroShop.Web.MvcApp.Models.Orders;
using MicroShop.Web.MvcApp.Models.Products;

namespace MicroShop.Web.MvcApp.Models;

internal static class Mapper
{
    public static UpsertCartDto ToUpsert(this ProductDto product, int count) =>
        new (null, product.Id, count);
    public static UpsertCartDto ToUpsert(this ProductDto product, int headerId, int count) =>
        new (headerId, product.Id, count);

    public static AddToCartDto ToAddToCart(this ProductDto product, int count) =>
        new(product.Id, product.Name, product.Description, product.CategoryName, product.Price, product.ImageUrl, count);
    public static UpsertCartDto ToUpsert(this AddToCartDto addToCartItem) =>
        new (null, addToCartItem.ProductId, addToCartItem.Count);
    public static UpsertCartDto ToUpsert(this AddToCartDto addToCartItem, int headerId) =>
        new (headerId, addToCartItem.ProductId, addToCartItem.Count);
    public static CartSummaryDto ToCartSummary(this CheckoutDto model) =>
        new(model.Id, model.CouponCode, model.Discount, model.CartTotal, model.Details.ToList());
    public static CreateOrderDto ToCreateOrder(this CheckoutDto checkout) =>
        new(checkout.Name, checkout.Email, checkout.ToCartSummary());
}
