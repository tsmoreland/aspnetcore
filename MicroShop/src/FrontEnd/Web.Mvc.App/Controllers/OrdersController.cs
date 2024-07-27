using System.IdentityModel.Tokens.Jwt;
using MicroShop.Web.Mvc.App.Models;
using MicroShop.Web.Mvc.App.Models.Orders;
using MicroShop.Web.Mvc.App.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroShop.Web.Mvc.App.Controllers;

/// <inheritdoc />
public sealed class OrdersController(IOrderService orderService) : Controller
{
    private const string AdminRole = "ADMIN";

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Detail(int orderId)
    {
        ResponseDto<OrderDto>? response = await orderService.GetOrder(orderId);
        if (response?.Success is not true)
        {
            return NotFound();
        }

        string? userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
        if (User.IsInRole(AdminRole) || userId == response.Data!.UserId)
        {
            return View(response.Data);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Processing(int orderId)
    {
        return await UpdateStatus(orderId, OrderUpdateStatus.Processing);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Complete(int orderId)
    {
        return await UpdateStatus(orderId, OrderUpdateStatus.Complete);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Cancel(int orderId)
    {
        return await UpdateStatus(orderId, OrderUpdateStatus.Cancelled);
    }

    private async Task<IActionResult> UpdateStatus(int orderId, OrderUpdateStatus ordersStatus,
        CancellationToken cancellationToken = default)
    {
        ResponseDto? response = await orderService.UpdateOrderStatus(orderId, ordersStatus, cancellationToken);
        if (response?.Success is true)
        {
            TempData["success"] = "Status updated";
        }
        else
        {
            TempData["error"] = "Unable to update status";
        }
        return RedirectToAction(nameof(Detail), new { orderId });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> History(string status)
    {
        string? userId = null;
        if (!User.IsInRole("ADMIN"))
        {
            userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
        }

        ResponseDto<IAsyncEnumerable<OrderDto>>? ordersResponse = await orderService.GetOrders(userId);
        if (ordersResponse?.Success is not true)
        {
            return Json(new { data = new List<OrderStatusDto>() });
        }

        IAsyncEnumerable<OrderDto> orders = ordersResponse.Data!;

        switch (status)
        {
            case "approved":
                orders = orders.Where(o => o.Status == OrderStatus.Approved);
                break;
            case "processing":
                orders = orders.Where(o => o.Status == OrderStatus.Processing);
                break;
            case "cancelled":
                orders = orders.Where(o => o.Status == OrderStatus.Cancelled);
                break;
            case "complete":
                orders = orders.Where(o => o.Status == OrderStatus.Complete);
                break;
        }

        return Json(new { data = await orders.OrderBy(o => o.Id).ToListAsync() });
    }
}
