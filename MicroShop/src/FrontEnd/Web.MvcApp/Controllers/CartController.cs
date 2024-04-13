using MicroShop.Web.MvcApp.Models;
using MicroShop.Web.MvcApp.Models.Cart;
using MicroShop.Web.MvcApp.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroShop.Web.MvcApp.Controllers;

[Authorize]
public sealed class CartController(ICartService cartService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await GetCartForCurrentUser());
    }

    public IActionResult EmailCart()
    {
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Remove([FromForm] int cartDetailsId)
    {
        if (cartDetailsId <= 0)
        {
            TempData["error"] = "Invalid Request";
            return RedirectToAction(nameof(Index));
        }

        ResponseDto? response = await cartService.RemoveFromCart(cartDetailsId);
        if (response?.Success is true)
        {
            TempData["success"] = "Item removed from cart";
            return RedirectToAction(nameof(Index));
        }

        TempData["error"] = "Unable to remove from cart";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ApplyCoupon(ApplyCouponDto model)
    {
        if (!ModelState.IsValid)
        {
            TempData["error"] = "Invalid Request";
            return RedirectToAction(nameof(Index));
        }

        ResponseDto? response = await cartService.ApplyCoupon(model.CartHeaderId, model.CouponCode);
        if (response?.Success is not true)
        {
            TempData["error"] = "Unable to update cart";
            return RedirectToAction(nameof(Index));
        }

        TempData["success"] = "Cart updated";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> RemoveCoupon(int cartDetailsId)
    {
        ResponseDto? response = await cartService.RemoveCoupon(cartDetailsId);
        if (response?.Success is not true)
        {
            TempData["error"] = "Unable to update cart";
            return RedirectToAction(nameof(Index));
        }

        TempData["success"] = "Cart updated";
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
