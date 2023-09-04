﻿using BethanysPieShop.Admin.Domain.Contracts;
using BethanysPieShop.Admin.Domain.Models;
using BethanysPieShop.MVC.App.Models.Categories;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Abstractions;

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
    public async Task<IActionResult> Index()
    {
        ListViewModel model = new(await _repository.GetSummaries().ToListAsync());
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        Category? model = await _repository.FindById(id, default);
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

    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] Guid id)
    {
        Category category = await _repository.FindById(id, default) ?? throw new ArgumentException("The category to update cannot be found");
        return View(new EditViewModel {Id = category.Id, Name = category.Name, Description = category.Description});
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromBody] EditViewModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _repository.Update(model.Id, model.Name, model.Description);
                await _repository.SaveChanges(default);
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
            _logger.LogError(ex, "An error occurred adding category");
            ModelState.AddModelError("", "Error occurred adding the category");
        }
        return View(model);
    }
}
