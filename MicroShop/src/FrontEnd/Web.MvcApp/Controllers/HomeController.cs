using System.Diagnostics;
using MicroShop.Web.MvcApp.Models;
using MicroShop.Web.MvcApp.Models.Products;
using MicroShop.Web.MvcApp.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroShop.Web.MvcApp.Controllers;

public class HomeController(IProductService productService, ILogger<HomeController> logger) : Controller
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

    [Authorize]
    [HttpPost]
    [ActionName("ProductDetails")]
    public IActionResult ProductDetails(ProductDto productDto)
    {
        return View(productDto);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
