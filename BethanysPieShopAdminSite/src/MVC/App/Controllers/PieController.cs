//
// Copyright © 2023 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using BethanysPieShop.Admin.Domain.Contracts;
using BethanysPieShop.Admin.Domain.Models;
using BethanysPieShop.Admin.Domain.Projections;
using BethanysPieShop.Admin.Domain.ValueObjects;
using BethanysPieShop.MVC.App.Models.Pies;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

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
        ListViewModel model = new(await _pieRepository.GetSummaries(PiesOrder.Name, false).ToListAsync());
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Paged(int? pageNumber)
    {
        const int pageSize = 10;
        pageNumber ??= 1;

        Page<PieSummary> page = await _pieRepository.GetSummaryPage(new PageRequest<PiesOrder>(pageNumber.Value, pageSize, PiesOrder.Name, false), default);
        return View(page);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        Pie? model = await _pieRepository.FindById(id, default);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        try
        {
            AddViewModel viewModel = new(await _categoryRepository.GetSummaries(CategoriesOrder.Name, false).ToListAsync());
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
            return View(new AddViewModel(await _categoryRepository.GetSummaries(CategoriesOrder.Name, false).ToListAsync()));
        }

        try
        {
            Category? category = await _categoryRepository.FindById(model.CategoryId, default);
            if (category is null)
            {
                ModelState.AddModelError("CategoryId", "Not Found");
                return View(new AddViewModel(await _categoryRepository.GetSummaries(CategoriesOrder.Name, false).ToListAsync()));
            }

            Pie pie = new(model.Name, model.Price, model.ShortDescription, model.LongDescription, model.AllergyInformation, model.ImageUrl, model.ImageThumbnailUrl, category);
            await _pieRepository.Add(pie, default);
            await _pieRepository.SaveChanges(default);

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
            _logger.LogError(ex, "An error occurred adding a pie");
            ModelState.AddModelError("", "Error occurred adding the pie");
        }

        return View(new AddViewModel(await _categoryRepository.GetSummaries(CategoriesOrder.Name, false).ToListAsync()));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        Pie? pie = await _pieRepository.FindById(id, default).ConfigureAwait(false);
        if (pie is null)
        {
            return NotFound();
        }

        EditViewModel viewModel = new(pie, await _categoryRepository.GetSummaries(CategoriesOrder.Name, false).ToListAsync());

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditViewModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await model.UpdateModel(_pieRepository, _categoryRepository, default);
                await _pieRepository.SaveChanges(default);
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
            _logger.LogError(ex, "An error occurred adding a pie");
            ModelState.AddModelError("", "Error occurred adding the pie");
        }

        return View(new EditViewModel(await _categoryRepository.GetSummaries(CategoriesOrder.Name, false).ToListAsync()) { Id = model.Id });
    }
}
