using System.Diagnostics;
using MicroShop.Web.Mvc.App.Models;
using MicroShop.Web.Mvc.App.Models.Cart;
using MicroShop.Web.Mvc.App.Models.Products;
using MicroShop.Web.Mvc.App.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroShop.Web.Mvc.App.Controllers;

public class HomeController(IProductService productService, ICartService cartService, ILogger<HomeController> logger) : Controller
{
    public async Task<IActionResult> Index()
    {
        ResponseDto<IEnumerable<ProductDto>>? response = await productService.GetProducts().ConfigureAwait(false);
        if (response?.Success == true)
        {
            return View(response.Data!.ToList());
        }
        else
        {
            TempData["error"] = response?.ErrorMessage;
            logger.LogError("Erorr occurred: {ErrorMessage} while retrieving products", response?.ErrorMessage ?? "(Unknown)");
            return View(new List<ProductDto>());
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ProductDetails([FromQuery] int productId)
    {
        ResponseDto<ProductDto>? response = await productService.GetProductById(productId);
        if (response is not { Success: true, Data: not null })
        {
            return NotFound();
        }
        return View(response.Data.ToAddToCart(1));
    }

    [Authorize]
    [HttpPost]
    [ActionName("ProductDetails")]
    public async Task<IActionResult> ProductDetails(AddToCartDto addToCart)
    {
        ResponseDto<CartSummaryDto>? response = await cartService.Upsert(addToCart.ToUpsert());
        if (response is not { Success: true, Data: not null })
        {
            TempData["error"] = "Error occurred attempting to add item to cart, please try again";
            return View(addToCart);
        }

        TempData["success"] = "Item has been added to the cart";
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
