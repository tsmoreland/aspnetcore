using BethanysPieShop.Admin.Domain.Contracts;
using BethanysPieShop.Admin.Domain.Models;
using BethanysPieShop.Admin.Domain.Projections;
using BethanysPieShop.Admin.Domain.ValueObjects;
using BethanysPieShop.MVC.App.Models.Categories;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShop.MVC.App.Controllers;

public sealed class CategoryController(ICategoryRepository repository, ILogger<CategoryController> logger) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ListViewModel model = new(await repository.GetSummaries(CategoriesOrder.Name, false).ToListAsync());
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Paged(int? pageNumber)
    {
        const int pageSize = 10;
        pageNumber ??= 1;

        Page<CategorySummary> page = await repository.GetSummaryPage(new PageRequest<CategoriesOrder>(pageNumber.Value, pageSize, CategoriesOrder.Name, false), default);
        return View(page);
    }

    [HttpGet]
    public async Task<IActionResult> PagedAndSorted(int? pageNumber, string sortBy, bool descending)
    {
        const int pageSize = 10;
        pageNumber ??= 1;

        ViewData["CustomSort"] = sortBy;
        ViewData["SortDirection"] = !descending;

        CategoriesOrder orderBy = CategoriesOrderFactory.FromString(sortBy);
        Page<CategorySummary> page = await repository.GetSummaryPage(new PageRequest<CategoriesOrder>(pageNumber.Value, pageSize, orderBy, descending), default);
        return View(page);
    }


    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        Category? model = await repository.FindById(id, default);
        return View(model);
    }

    [HttpGet]
    public IActionResult Add()
    {
        AddViewModel model = new();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Add([Bind("Name", "Description", "DateAdded")] AddViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            Category category = new(model.Name, model.Description) { DateAdded = model.DateAdded };
            await repository.Add(category, default);
            await repository.SaveChanges(default);
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
            logger.LogError(ex, "An error occurred adding category");
            ModelState.AddModelError("", "Error occurred adding the category");
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] Guid id)
    {
        Category category = await repository.FindById(id, default) ?? throw new ArgumentException("The category to update cannot be found");
        return View(new EditViewModel {Id = category.Id, Name = category.Name, Description = category.Description});
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromBody] EditViewModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await repository.Update(model.Id, model.Name, model.Description);
                await repository.SaveChanges(default);
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();
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
            logger.LogError(ex, "An error occurred adding category");
            ModelState.AddModelError("", "Error occurred adding the category");
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        Category? category = await repository.FindById(id, default);
        return View(category);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id is null)
        {
            ViewData["ErrorMessage"] = "Deleting the category failed, invalid ID!";
            return View();
        }

        Category? category = await repository.FindById(id.Value, default);
        if (category is null)
        {
            ViewData["ErrorMessage"] = "Deleting the category failed, invalid ID!";
            return View();
        }
        try
        {
            await repository.Delete(id.Value, default);
            await repository.SaveChanges(default);
            TempData["CategoryDeleted"] = "Category deleted successfully";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = $"Deleting the category failed, please try again! Error: {ex.Message}";
        }

        return View(await repository.FindById(id.Value, default));
    }
}
