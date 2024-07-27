using MicroShop.Web.Mvc.App.Models.Cart;

namespace MicroShop.Web.Mvc.App.Models.Orders;

public sealed record class CreateOrderDto(string Name, string EmailAddress, CartSummaryDto Cart);
