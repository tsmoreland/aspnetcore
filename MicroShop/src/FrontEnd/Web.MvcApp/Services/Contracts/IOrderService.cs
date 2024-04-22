using MicroShop.Web.MvcApp.Models;
using MicroShop.Web.MvcApp.Models.Orders;

namespace MicroShop.Web.MvcApp.Services.Contracts;

public interface IOrderService
{
    Task<ResponseDto<OrderSummaryDto>?> Create(CreateOrderDto model, CancellationToken cancellationToken = default);
}
