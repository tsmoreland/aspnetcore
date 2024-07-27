using System.Security.Claims;
using MicroShop.Web.Mvc.App.Models;
using MicroShop.Web.Mvc.App.Models.Cart;
using MicroShop.Web.Mvc.App.Models.Orders;
using MicroShop.Web.Mvc.App.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroShop.Web.Mvc.App.Controllers;

[Authorize]
public sealed class CartController(ICartService cartService, IOrderService orderService) : Controller
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
            TempData["success"] = "Email will be processed and sent soon";
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

    [HttpPost]
    public async Task<IActionResult> Checkout(CheckoutDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        CreateOrderDto order = model.ToCreateOrder();

        ResponseDto<OrderSummaryDto>? response = await orderService.CreateOrder(order);
        if (response?.Success != true)
        {
            TempData["error"] = "Errors and warnings must be addressed";
            return View(model);
        }

        // get stripe session and redirect to place order (art
        ResponseDto<StripeResponseDto>? stripeResponse = await orderService.CreateStripeSession(BuildStripeRequest(response.Data!));
        if (stripeResponse?.Success != true)
        {
            // TODO: if this fails we need to cancel the order or allow for it to be remembered and payment retried.  It may be enough to update checkout dto with order id
            TempData["error"] = "Errors and warnings must be addressed";
            return View(model);
        }

        Response.Headers.Append("Location", stripeResponse.Data!.StripeSessionUrl);
        return new StatusCodeResult(StatusCodes.Status303SeeOther);

        StripeRequest BuildStripeRequest(OrderSummaryDto orderSummary)
        {
            int orderId = orderSummary.Id;
            string domain = $"{Request.Scheme}://{Request.Host.Value}";

            string approvedUrl = $"{domain}/cart/confirmation?orderId={orderId}";
            string cancelUrl = $"{domain}/cart/checkout";

            return new StripeRequest(new Uri(approvedUrl), new Uri(cancelUrl), orderSummary);
        }
    }


    [HttpGet("orders/{orderId}")]
    public async Task<IActionResult> Confirmation(int orderId)
    {
        ResponseDto<OrderSummaryDto>? orderSummary = await orderService.GetOrderSummary(orderId);
        if (orderSummary?.Success != true || orderSummary?.Data?.Status != OrderStatus.Approved)
        {
            // TODO redirect to error page specific to order status, to allow for payment to continue or be cancelled
            return View(orderId);
        }

        return View(orderId);
    }

    private async Task<CartSummaryDto> GetCartForCurrentUser()
    {
        ResponseDto<CartSummaryDto>? responseModel = await cartService.GetCartForCurrentUser();
        return responseModel is { Success: true, Data: not null }
            ? responseModel.Data
            : CartSummaryDto.CreateNew();
    }
}
