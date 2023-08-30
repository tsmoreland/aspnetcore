using BethanysPieShop.Admin.Domain.Contracts;
using BethanysPieShop.MVC.App.Models.Categories;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShop.MVC.App.Controllers;

public sealed class CategoryController : Controller
{
    private readonly ICategoryRepository _repository;

    /// <inheritdoc />
    public CategoryController(ICategoryRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpHead]
    public async Task<IActionResult> Index()
    {
        ListViewModel model = new(await _repository.GetSummaries().ToListAsync());
        return View(model);
    }
}
