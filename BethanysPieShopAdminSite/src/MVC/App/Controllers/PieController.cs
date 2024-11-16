using BethanysPieShop.Admin.Domain.Contracts;
using BethanysPieShop.Admin.Domain.Models;
using BethanysPieShop.Admin.Domain.Projections;
using BethanysPieShop.Admin.Domain.ValueObjects;
using BethanysPieShop.MVC.App.Models.Pies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.MVC.App.Controllers;

public sealed class PieController : Controller
{
    private readonly IPieRepository _pieRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<PieController> _logger;

    /// <inheritdoc />
    public PieController(IPieRepository pieRepository, ICategoryRepository categoryRepository, ILogger<PieController> logger)
    {
        _pieRepository = pieRepository;
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ListViewModel model = new(await _pieRepository.GetSummaries(PiesOrder.Name, false).ToListAsync().ConfigureAwait(false));
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Paged(int? pageNumber)
    {
        const int pageSize = 10;
        pageNumber ??= 1;

        var page = await _pieRepository.GetSummaryPage(new PageRequest<PiesOrder>(pageNumber.Value, pageSize, PiesOrder.Name, false), default).ConfigureAwait(false);
        return View(page);
    }

    [HttpGet]
    public async Task<IActionResult> PagedAndSorted(int? pageNumber, string sortBy, bool descending)
    {
        const int pageSize = 10;
        pageNumber ??= 1;

        ViewData["CustomSort"] = sortBy;
        // TODO: track sort direction by property in ViewData, when we change column is should revert to ascending
        ViewData["SortDirection"] = !descending;

        var orderBy = PiesOrderFactory.FromString(sortBy);
        var page = await _pieRepository.GetSummaryPage(new PageRequest<PiesOrder>(pageNumber.Value, pageSize, orderBy, descending), default).ConfigureAwait(false);
        return View(page);
    }

    [HttpGet]
    public async Task<IActionResult> Search(string? query, Guid? categoryId)
    {
        IEnumerable<SelectListItem> selectItems = new SelectList(
            await _categoryRepository.GetSummaries(CategoriesOrder.Name, false).ToListAsync().ConfigureAwait(false), "Id", "Name", null);
        if (query is not null)
        {
            var pies = await _pieRepository.Search(query, categoryId).ToListAsync().ConfigureAwait(false);
            return View(new SearchViewModel { Items = pies, Categories = selectItems, Query = query, CategoryId = categoryId });
        }

        return View(new SearchViewModel { Items = new List<PieSummary>(), Categories = selectItems, Query = string.Empty, CategoryId = null });
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var model = await _pieRepository.FindById(id, default).ConfigureAwait(false);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        try
        {
            AddViewModel viewModel = new(await _categoryRepository.GetSummaries(CategoriesOrder.Name, false).ToListAsync().ConfigureAwait(false));
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred loading category summaries");
            ViewData["ErrorMessage"] = ex.Message;
            return View(new AddViewModel());
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(new AddViewModel(await _categoryRepository.GetSummaries(CategoriesOrder.Name, false).ToListAsync().ConfigureAwait(false)));
        }

        try
        {
            var category = await _categoryRepository.FindById(model.CategoryId, default).ConfigureAwait(false);
            if (category is null)
            {
                ModelState.AddModelError("CategoryId", "Not Found");
                return View(new AddViewModel(await _categoryRepository.GetSummaries(CategoriesOrder.Name, false).ToListAsync().ConfigureAwait(false)));
            }

            Pie pie = new(model.Name, model.Price, model.ShortDescription, model.LongDescription, model.AllergyInformation, model.ImageUrl, model.ImageThumbnailUrl, category);
            await _pieRepository.Add(pie, default).ConfigureAwait(false);
            await _pieRepository.SaveChanges(default).ConfigureAwait(false);

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
            _logger.LogError(ex, "An error occurred adding a pie");
            ModelState.AddModelError("", "Error occurred adding the pie");
        }

        return View(new AddViewModel(await _categoryRepository.GetSummaries(CategoriesOrder.Name, false).ToListAsync().ConfigureAwait(false)));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        Pie? pie = await _pieRepository.FindById(id, default).ConfigureAwait(false);
        if (pie is null)
        {
            return NotFound();
        }

        EditViewModel viewModel = new(pie, await _categoryRepository.GetSummaries(CategoriesOrder.Name, false).ToListAsync().ConfigureAwait(false));

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await model.UpdateModel(_pieRepository, _categoryRepository, default).ConfigureAwait(false);
            await _pieRepository.SaveChanges(default).ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException ex)
        {
            var failedEntity = ex.Entries.Single();
            var entityValue = (Pie)failedEntity.Entity;
            var databasePie = await failedEntity.GetDatabaseValuesAsync().ConfigureAwait(false);
            if (databasePie is null)
            {
                ModelState.AddModelError(string.Empty, "The pie was already deleted by another user.");
            }
            else
            {
                var databaseValue = (Pie)databasePie.ToObject();
                if (databaseValue.Name != entityValue.Name)
                {
                    ModelState.AddModelError(nameof(Pie.Name), $"Current value: {databaseValue.Name}");
                }
                //... for the remaining properties

                // Then high level error message
                ModelState.AddModelError(string.Empty, "The pie was modified already by another user.");
                ModelState.Remove(nameof(EditViewModel.ConcurrencyToken));
                return View(new EditViewModel(model, await _categoryRepository.GetSummaries(CategoriesOrder.Name, false).ToListAsync().ConfigureAwait(false))
                {
                    ConcurrencyToken = Convert.ToBase64String(databaseValue.ConcurrencyToken)
                });
            }
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
            _logger.LogError(ex, "An error occurred adding a pie");
            ModelState.AddModelError("", "Error occurred adding the pie");
        }

        return View(new EditViewModel(model,  await _categoryRepository.GetSummaries(CategoriesOrder.Name, false).ToListAsync().ConfigureAwait(false)));
    }
}
