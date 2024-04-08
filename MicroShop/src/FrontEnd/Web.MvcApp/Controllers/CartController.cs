using MicroShop.Web.MvcApp.Models;
using MicroShop.Web.MvcApp.Models.Cart;
using MicroShop.Web.MvcApp.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroShop.Web.MvcApp.Controllers;

public sealed class CartController(ICartService cartService) : Controller
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Index()
    {
        return View(await GetCartForCurrentUser());
    }

    public IActionResult EmailCart()
    {
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Remove()
    {
        return RedirectToAction(nameof(Index));
    }

    public IActionResult ApplyCoupon()
    {
        return RedirectToAction(nameof(Index));
    }

    public IActionResult RemoveCoupon()
    {
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Checkout()
    {
        return RedirectToAction(nameof(Index));
    }

    private async Task<CartSummaryDto> GetCartForCurrentUser()
    {
        ResponseDto<CartSummaryDto>? responseModel = await cartService.GetCartForCurrentUser();
        return responseModel is { Success: true, Data: not null }
            ? responseModel.Data
            : CartSummaryDto.CreateNew();
    }
}
