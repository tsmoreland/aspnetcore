using MicroShop.Web.MvcApp.Models.Cart;

namespace MicroShop.Web.MvcApp.Models.Orders;

public sealed record class CreateOrderDto(string Name, string EmailAddress, CartSummaryDto Cart);
