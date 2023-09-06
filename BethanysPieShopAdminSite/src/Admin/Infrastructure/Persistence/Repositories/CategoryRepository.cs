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
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.Admin.Infrastructure.Persistence.Repositories;

[ReadOnlyRepository("BethanysPieShop.Admin.Domain.Models.Category", "BethanysPieShop.Admin.Domain.Projections.CategorySummary", "BethanysPieShop.Admin.Domain.ValueObjects.CategoriesOrder")]
[WritableRepository("BethanysPieShop.Admin.Domain.Models.Category")]
public sealed partial class CategoryRepository : ICategoryRepository, ICategoryReadOnlyRepository
{
    private readonly AdminDbContext _dbContext;
    private DbSet<Category> Entities => _dbContext.Categories;

    public CategoryRepository(AdminDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public partial IAsyncEnumerable<Category> GetAll(CategoriesOrder orderBy, bool descending);

    /// <inheritdoc />
    public partial IAsyncEnumerable<CategorySummary> GetSummaries(CategoriesOrder orderBy, bool descending);

    /// <inheritdoc />
    public partial Task<Category?> FindById(Guid id, CancellationToken cancellationToken);

    private static IQueryable<CategorySummary> GetSummaries(IQueryable<Category> categories)
    {
        return categories
            .Select(e => new CategorySummary(e.Id, e.Name, e.DateAdded));
    }

    private static IQueryable<Category> GetIncludesForFind(IQueryable<Category> queryable)
    {
        return queryable.Include(e => e.Pies);
    }
    private async ValueTask GetIncludesForFindTracked(Category entity, CancellationToken cancellationToken)
    {
        await _dbContext.Entry(entity).Collection(e => e.Pies).LoadAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public partial ValueTask Add(Category entity, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public partial ValueTask Update(Category entity, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public async ValueTask Update(Guid id, string name, string? description, CancellationToken cancellationToken = default)
    {
        Category category = await FindById(id, default) ?? throw new ArgumentException("category not found", nameof(id));
        category.Name = name;
        category.Description = description;
        await ValidateUpdateOrThrow(category, cancellationToken).ConfigureAwait(false);

        foreach (Pie pie in category.Pies)
        {
            pie.Category = category; // trigger update to category name
        }
    }

    /// <inheritdoc/>
    public partial void Delete(Category entity);

    /// <inheritdoc/>
    public partial ValueTask Delete(Guid id, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public partial ValueTask SaveChanges(CancellationToken cancellationToken);

    private async ValueTask ValidateAddOrThrow(Category entity, CancellationToken cancellationToken)
    {
        if (await Entities.AsNoTracking().AnyAsync(e => e.Name == entity.Name, cancellationToken))
        {
            throw new ValidationException("Category with same name already exists",
                new[] { new ValidationFailure("Name", "Category with same name already exists") });
        }
    }
    private async ValueTask ValidateUpdateOrThrow(Category entity, CancellationToken cancellationToken)
    {
        if (await Entities.AsNoTracking().AnyAsync(e => e.Name == entity.Name && e.Id != entity.Id, cancellationToken))
        {
            throw new ValidationException("Category with same name already exists",
                new[] { new ValidationFailure("Name", "Category with same name already exists") });
        }
    }

    private async ValueTask<(bool allowed, string reason)> AllowsDelete(Guid id, CancellationToken cancellationToken)
    {
        bool allowed = !await _dbContext.Pies.AsNoTracking().Where(e => e.CategoryId == id).AnyAsync(cancellationToken);

        return (allowed, !allowed ? "Pies exist in this category.  Delete all the pies in this category before deleting the category" : string.Empty);
    }
}
