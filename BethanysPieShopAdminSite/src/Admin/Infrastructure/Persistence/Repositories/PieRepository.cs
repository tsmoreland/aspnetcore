using BethanysPieShop.Admin.Domain.Contracts;
using BethanysPieShop.Admin.Domain.Models;
using BethanysPieShop.Admin.Domain.Projections;
using BethanysPieShop.Admin.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.Admin.Infrastructure.Persistence.Repositories;

[ReadOnlyRepository("BethanysPieShop.Admin.Domain.Models.Pie", "BethanysPieShop.Admin.Domain.Projections.PieSummary", "BethanysPieShop.Admin.Domain.ValueObjects.PiesOrder")]
[WritableRepository("BethanysPieShop.Admin.Domain.Models.Pie")]
public sealed partial class PieRepository(AdminDbContext dbContext) : IPieRepository, IPieReadOnlyRepository
{
    private DbSet<Pie> Entities => dbContext.Pies;

    /// <inheritdoc />
    public partial IAsyncEnumerable<Pie> GetAll(PiesOrder orderBy, bool descending);

    /// <inheritdoc />
    public partial IAsyncEnumerable<PieSummary> GetSummaries(PiesOrder orderBy, bool descending);

    /// <inheritdoc />
    public partial ValueTask<Page<PieSummary>> GetSummaryPage(PageRequest<PiesOrder> request, CancellationToken cancellationToken);

    /// <inheritdoc />
    public partial Task<Pie?> FindById(Guid id, CancellationToken cancellationToken);

    /// <inheritdoc />
    public IAsyncEnumerable<PieSummary> Search(string query, Guid? categoryId)
    {
        var queryable = Entities.AsNoTracking();

        if (query is { Length: > 0 })
        {
            queryable = queryable.Where(e => e.Name.Contains(query) || e.ShortDescription!.Contains(query) || e.LongDescription!.Contains(query));
        }

        if (categoryId is not null)
        {
            queryable = queryable.Where(e => e.Id == categoryId);
        }

        return GetSummaries(queryable).AsAsyncEnumerable();
    }

    /// <inheritdoc />
    public void SetOriginalConcurrencyToken(Pie entity, byte[] concurrencyToken)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(concurrencyToken);
        dbContext.Entry(entity).Property(nameof(Pie.ConcurrencyToken)).OriginalValue = concurrencyToken;
    }

    private static IQueryable<PieSummary> GetSummaries(IQueryable<Pie> pies)
    {
        return pies
            .Select(e => new PieSummary(e.Id, e.Name, e.ShortDescription, e.ImageThumbnailFilename, e.CategoryId, e.CategoryName));
    }

    private static IQueryable<Pie> GetIncludesForFind(IQueryable<Pie> queryable)
    {
        return queryable.Include(e => e.Ingredients);
    }
    private async ValueTask GetIncludesForFindTracked(Pie entity, CancellationToken cancellationToken)
    {
        await dbContext.Entry(entity).Collection(e => e.Ingredients).LoadAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public partial ValueTask Add(Pie entity, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public partial ValueTask Update(Pie entity, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public partial void Delete(Pie entity);

    /// <inheritdoc/>
    public partial ValueTask Delete(Guid id, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public partial ValueTask SaveChanges(CancellationToken cancellationToken);

#pragma warning disable CA1822
    private static ValueTask ValidateAddOrThrow(Pie entity, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }
    private static ValueTask ValidateUpdateOrThrow(Pie entity, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }
    private ValueTask<(bool allowed, string reason)> AllowsDelete(Guid id, CancellationToken cancellationToken)
#pragma warning restore CA1822
    {
        return ValueTask.FromResult((true, string.Empty));
    }
} 
