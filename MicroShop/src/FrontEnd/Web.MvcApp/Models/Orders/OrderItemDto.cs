using System.ComponentModel.DataAnnotations;

namespace MicroShop.Web.MvcApp.Models.Orders;

public sealed record class OrderItemDto(
    [property: Required] int Id,
    [property: Required] int ProductId,
    [property: Required] string ProductName,
    [property: Required] double Price,
    [property: Required] int Count);
