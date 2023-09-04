using BethanysPieShop.Admin.Domain.Contracts;
using BethanysPieShop.Admin.Domain.Models;
using BethanysPieShop.MVC.App.Models.Categories;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShop.MVC.App.Controllers;

public sealed class CategoryController : Controller
{
    private readonly ICategoryRepository _repository;
    private readonly ILogger<CategoryController> _logger;

    /// <inheritdoc />
    public CategoryController(ICategoryRepository repository, ILogger<CategoryController> logger)
    {
        _repository = repository;
        _logger = logger;
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
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            Category category = new(model.Name, model.Description) { DateAdded = model.DateAdded };
            await _repository.Add(category, default);
            await _repository.SaveChanges(default);
            return RedirectToAction(nameof(Index));
        }
        catch (FluentValidation.ValidationException ex)
        {
            foreach (ValidationFailure? error in ex.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred adding category");
            ModelState.AddModelError("", "Error occurred adding the category");
        }

        return View(model);
    }
}
