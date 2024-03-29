﻿//
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

using System.Runtime.InteropServices;
using BethanysPieShop.Admin.Domain.Contracts;
using BethanysPieShop.Admin.Domain.Models;
using BethanysPieShop.Admin.Domain.Projections;
using BethanysPieShop.Admin.Domain.ValueObjects;
using BethanysPieShop.MVC.App.Models.Pies;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

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
    public async Task<IActionResult> PagedAndSorted(int? pageNumber, string sortBy, bool descending)
    {
        const int pageSize = 10;
        pageNumber ??= 1;

        ViewData["CustomSort"] = sortBy;
        // TODO: track sort direction by property in ViewData, when we change column is should revert to ascending
        ViewData["SortDirection"] = !descending;

        PiesOrder orderBy = PiesOrderFactory.FromString(sortBy);
        Page<PieSummary> page = await _pieRepository.GetSummaryPage(new PageRequest<PiesOrder>(pageNumber.Value, pageSize, orderBy, descending), default);
        return View(page);
    }

    [HttpGet]
    public async Task<IActionResult> Search(string? query, Guid? categoryId)
    {
        IEnumerable<SelectListItem> selectItems = new SelectList(
            await _categoryRepository.GetSummaries(CategoriesOrder.Name, false).ToListAsync(), "Id", "Name", null);
        if (query is not null)
        {
            List<PieSummary> pies = await _pieRepository.Search(query, categoryId).ToListAsync();
            return View(new SearchViewModel { Items = pies, Categories = selectItems, Query = query, CategoryId = categoryId });
        }

        return View(new SearchViewModel { Items = new List<PieSummary>(), Categories = selectItems, Query = string.Empty, CategoryId = null });
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
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await model.UpdateModel(_pieRepository, _categoryRepository, default);
            await _pieRepository.SaveChanges(default);
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException ex)
        {
            EntityEntry failedEntity = ex.Entries.Single();
            Pie entityValue = (Pie)failedEntity.Entity;
            PropertyValues? databasePie = await failedEntity.GetDatabaseValuesAsync();
            if (databasePie is null)
            {
                ModelState.AddModelError(string.Empty, "The pie was already deleted by another user.");
            }
            else
            {
                Pie databaseValue = (Pie)databasePie.ToObject();
                if (databaseValue.Name != entityValue.Name)
                {
                    ModelState.AddModelError(nameof(Pie.Name), $"Current value: {databaseValue.Name}");
                }
                //... for the remaining properties

                // Then high level error message
                ModelState.AddModelError(string.Empty, "The pie was modified already by another user.");
                ModelState.Remove(nameof(EditViewModel.ConcurrencyToken));
                return View(new EditViewModel(model, await _categoryRepository.GetSummaries(CategoriesOrder.Name, false).ToListAsync())
                {
                    ConcurrencyToken = Convert.ToBase64String(databaseValue.ConcurrencyToken)
                });
            }
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

        return View(new EditViewModel(model,  await _categoryRepository.GetSummaries(CategoriesOrder.Name, false).ToListAsync()));
    }
}
