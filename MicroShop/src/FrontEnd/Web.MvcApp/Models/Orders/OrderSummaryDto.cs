using System.ComponentModel.DataAnnotations;

namespace MicroShop.Web.MvcApp.Models.Orders;

public sealed record OrderSummaryDto(
    [property: Required] int Id,
    [property: Required] OrderStatus Status,
    string? CouponCode,
    [property: Required] double Discount,
    [property: Required] double OrderTotal,
    IEnumerable<OrderItemDto> Details);
