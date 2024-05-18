using System.IdentityModel.Tokens.Jwt;
using MicroShop.Web.MvcApp.Models;
using MicroShop.Web.MvcApp.Models.Orders;
using MicroShop.Web.MvcApp.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroShop.Web.MvcApp.Controllers;

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
    public async Task<IActionResult> History()
    {
        string? userId = null;
        if (!User.IsInRole("ADMIN"))
        {
            userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
        }

        ResponseDto<IAsyncEnumerable<OrderStatusDto>>? ordersResponse = await orderService.GetOrderStatus(userId);
        return ordersResponse?.Success is true
            ? Json(new { data = await ordersResponse.Data!.ToListAsync() })
            : Json(new { data = new List<OrderStatusDto>() });
        }
    }
}
