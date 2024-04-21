using MicroShop.Web.MvcApp.Models;
using MicroShop.Web.MvcApp.Models.Cart;
using MicroShop.Web.MvcApp.Models.Orders;

namespace MicroShop.Web.MvcApp.Services.Contracts;

public interface IOrderService
{
    Task<ResponseDto<OrderSummaryDto>?> Create(CartSummaryDto cart, CancellationToken cancellationToken = default);
}
