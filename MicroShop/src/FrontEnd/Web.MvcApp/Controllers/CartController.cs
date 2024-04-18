using System.Security.Claims;
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

    [HttpPost]
    public async Task<IActionResult> EmailCart()
    {
        ResponseDto? response = await cartService.EmailCartToCurrentUser();
        if (response?.Success is true)
        {
            TempData["success"] = "Email will be procesed and sent soon";
            return RedirectToAction(nameof(Index));
        }

        TempData["error"] = "Unable to e-mail cart at this time.";
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

    [HttpGet]
    public async Task<IActionResult> Checkout(HttpContext context)
    {
        string? name = context.User.Identity?.Name;
        string? email = context.User.FindFirstValue(ClaimTypes.Email);

        if (name is { Length: > 0 } && email is { Length: > 0 })
        {
            return View(new CheckoutDto(name, email, await GetCartForCurrentUser()));
        }

        TempData["error"] = "user not found";
        return RedirectToAction(nameof(Index));

    }

    public async Task<IActionResult> Checkout(CheckoutDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await Task.Delay(1).ConfigureAwait(false); // temporary

        TempData["success"] = "Order submitted successfully.";
        return RedirectToAction("Index");
    }

    private async Task<CartSummaryDto> GetCartForCurrentUser()
    {
        ResponseDto<CartSummaryDto>? responseModel = await cartService.GetCartForCurrentUser();
        return responseModel is { Success: true, Data: not null }
            ? responseModel.Data
            : CartSummaryDto.CreateNew();
    }
}
