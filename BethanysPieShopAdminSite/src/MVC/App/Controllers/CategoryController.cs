using BethanysPieShop.Admin.Domain.Contracts;
using BethanysPieShop.MVC.App.Models;
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

    public async Task<IActionResult> Index()
    {
        CategoryListViewModel model = new(await _repository.GetSummaries().ToListAsync());
        return View(model);
    }
}
