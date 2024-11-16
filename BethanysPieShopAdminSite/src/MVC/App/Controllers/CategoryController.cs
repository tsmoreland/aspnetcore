using BethanysPieShop.Admin.Domain.Contracts;
using BethanysPieShop.Admin.Domain.Models;
using BethanysPieShop.Admin.Domain.ValueObjects;
using BethanysPieShop.MVC.App.Models.Categories;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShop.MVC.App.Controllers;

public sealed class CategoryController(ICategoryRepository repository, ILogger<CategoryController> logger) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ListViewModel model = new(await repository.GetSummaries(CategoriesOrder.Name, false).ToListAsync().ConfigureAwait(false));
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Paged(int? pageNumber)
    {
        const int pageSize = 10;
        pageNumber ??= 1;

        var page = await repository.GetSummaryPage(new PageRequest<CategoriesOrder>(pageNumber.Value, pageSize, CategoriesOrder.Name, false), default).ConfigureAwait(false);
        return View(page);
    }

    [HttpGet]
    public async Task<IActionResult> PagedAndSorted(int? pageNumber, string sortBy, bool descending)
    {
        const int pageSize = 10;
        pageNumber ??= 1;

        ViewData["CustomSort"] = sortBy;
        ViewData["SortDirection"] = !descending;

        var orderBy = CategoriesOrderFactory.FromString(sortBy);
        var page = await repository.GetSummaryPage(new PageRequest<CategoriesOrder>(pageNumber.Value, pageSize, orderBy, descending), default).ConfigureAwait(false);
        return View(page);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var model = await repository.FindById(id, default).ConfigureAwait(false);
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
            await repository.Add(category, default).ConfigureAwait(false);
            await repository.SaveChanges(default).ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }
        catch (FluentValidation.ValidationException ex)
        {
            foreach (var error in ex.Errors)
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
        var category = await repository.FindById(id, default).ConfigureAwait(false) ?? throw new ArgumentException("The category to update cannot be found");
        return View(new EditViewModel { Id = category.Id, Name = category.Name, Description = category.Description });
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromBody] EditViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await repository.Update(model.Id, model.Name, model.Description).ConfigureAwait(false);
            await repository.SaveChanges(default).ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }
        catch (FluentValidation.ValidationException ex)
        {
            foreach (var error in ex.Errors)
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
        var category = await repository.FindById(id, default).ConfigureAwait(false);
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

        var category = await repository.FindById(id.Value, default).ConfigureAwait(false);
        if (category is null)
        {
            ViewData["ErrorMessage"] = "Deleting the category failed, invalid ID!";
            return View();
        }
        try
        {
            await repository.Delete(id.Value, default).ConfigureAwait(false);
            await repository.SaveChanges(default).ConfigureAwait(false);
            TempData["CategoryDeleted"] = "Category deleted successfully";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = $"Deleting the category failed, please try again! Error: {ex.Message}";
        }

        return View(await repository.FindById(id.Value, default).ConfigureAwait(false));
    }
}
