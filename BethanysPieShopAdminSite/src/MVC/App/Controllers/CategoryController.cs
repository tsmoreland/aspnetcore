using BethanysPieShop.Admin.Domain.Contracts;
using BethanysPieShop.Admin.Domain.Models;
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

    public IActionResult Add()
    {
        AddViewModel model = new();
        return View(model);
    }

    public async Task<IActionResult> Add([Bind("Name", "Description", "DateAdded")] AddViewModel model)
    {
        Category category = new(model.Name, model.Descripton) { DateAdded = model.DateAdded };
        await _repository.Add(category, default);
        await _repository.SaveChanges(default);
        return RedirectToAction(nameof(Index));
    }
}
