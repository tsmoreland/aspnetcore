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
public sealed partial class CategoryRepository(AdminDbContext dbContext)
    : ICategoryRepository, ICategoryReadOnlyRepository
{
    private DbSet<Category> Entities => dbContext.Categories;

    /// <inheritdoc />
    public partial IAsyncEnumerable<Category> GetAll(CategoriesOrder orderBy, bool descending);

    /// <inheritdoc />
    public partial IAsyncEnumerable<CategorySummary> GetSummaries(CategoriesOrder orderBy, bool descending);

    /// <inheritdoc />
    public partial ValueTask<Page<CategorySummary>> GetSummaryPage(PageRequest<CategoriesOrder> request, CancellationToken cancellationToken);

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
        await dbContext.Entry(entity).Collection(e => e.Pies).LoadAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public partial ValueTask Add(Category entity, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public partial ValueTask Update(Category entity, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public async ValueTask Update(Guid id, string name, string? description, CancellationToken cancellationToken = default)
    {
        var category = await FindById(id, default).ConfigureAwait(false) ?? throw new ArgumentException("category not found", nameof(id));
        category.Name = name;
        category.Description = description;
        await ValidateUpdateOrThrow(category, cancellationToken).ConfigureAwait(false);

        foreach (var pie in category.Pies)
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
        if (await Entities.AsNoTracking().AnyAsync(e => e.Name == entity.Name, cancellationToken).ConfigureAwait(false))
        {
            throw new ValidationException("Category with same name already exists",
                [ new ValidationFailure("Name", "Category with same name already exists") ]);
        }
    }
    private async ValueTask ValidateUpdateOrThrow(Category entity, CancellationToken cancellationToken)
    {
        if (await Entities.AsNoTracking().AnyAsync(e => e.Name == entity.Name && e.Id != entity.Id, cancellationToken).ConfigureAwait(false))
        {
            throw new ValidationException("Category with same name already exists",
                [ new ValidationFailure("Name", "Category with same name already exists") ]);
        }
    }

    private async ValueTask<(bool allowed, string reason)> AllowsDelete(Guid id, CancellationToken cancellationToken)
    {
        var allowed = !await dbContext.Pies.AsNoTracking().Where(e => e.CategoryId == id).AnyAsync(cancellationToken).ConfigureAwait(false);
        return (allowed, !allowed ? "Pies exist in this category.  Delete all the pies in this category before deleting the category" : string.Empty);
    }
}
