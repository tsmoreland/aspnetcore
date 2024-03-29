using Microsoft.AspNetCore.Mvc;

namespace MicroShop.Web.MvcApp.Controllers;

public sealed class ProductController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
