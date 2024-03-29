using MicroShop.Web.MvcApp.Models;
using MicroShop.Web.MvcApp.Models.Products;
using MicroShop.Web.MvcApp.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MicroShop.Web.MvcApp.Controllers;

public sealed class ProductController(IProductService productService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ResponseDto<IEnumerable<ProductDto>>? response = await productService.GetProducts();
        if (response?.Success is not true || response?.Data is null)
        {
            TempData["error"] = response?.ErrorMessage;
            return View(new List<ProductDto>());
        }

        List<ProductDto> products = response.Data.ToList();
        return View(products);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new AddProductDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create(AddProductDto product)
    {
        if (!ModelState.IsValid)
        {
            return View(product);
        }

        ResponseDto<ProductDto>? response = await productService.AddProduct(product);
        if (response?.Success is true)
        {
            TempData["success"] = "Product Created Successfully";
            return RedirectToAction(nameof(Index));
        }

        TempData["error"] = response?.ErrorMessage;
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductDto productDto)
    {
        if (!ModelState.IsValid)
        {
            return View(productDto);
        }

        ResponseDto? response = await productService.UpdateProduct(productDto);

        if (response?.Success is true)
        {
            TempData["success"] = "Product updated successfully";
            return RedirectToAction(nameof(ProductIndex));
        }

        TempData["error"] = response?.ErrorMessage;
        return View(productDto);
    }


    [HttpGet]
    public async Task<IActionResult> Delete(int productId)
    {
        ResponseDto<ProductDto>? response = await productService.GetProductById(productId);
        if (response is { Success: true, Data: not null })
        {
            return View(response.Data);
        }

        TempData["error"] = response?.ErrorMessage;
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(ProductDto product)
    {
        ResponseDto<ProductDto>? response = await productService.DeleteProduct(product.Id);
        if (response?.Success is true)
        {
            TempData["success"] = "Product deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        TempData["error"] = response?.ErrorMessage;
        return View(product);
    }
}
