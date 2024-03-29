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

using BethanysPieShop.Admin.Domain.Contracts;
using BethanysPieShop.Admin.Domain.Models;
using BethanysPieShop.Admin.Domain.Projections;
using BethanysPieShop.Admin.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.Admin.Infrastructure.Persistence.Repositories;

[ReadOnlyRepository("BethanysPieShop.Admin.Domain.Models.Pie", "BethanysPieShop.Admin.Domain.Projections.PieSummary", "BethanysPieShop.Admin.Domain.ValueObjects.PiesOrder")]
[WritableRepository("BethanysPieShop.Admin.Domain.Models.Pie")]
public sealed partial class PieRepository : IPieRepository, IPieReadOnlyRepository
{
    private readonly AdminDbContext _dbContext;
    private DbSet<Pie> Entities => _dbContext.Pies;

    public PieRepository(AdminDbContext dbContext)
    {
        _dbContext = dbContext;
    }

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
        IQueryable<Pie> queryable = Entities.AsNoTracking();

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
        _dbContext.Entry(entity).Property(nameof(Pie.ConcurrencyToken)).OriginalValue = concurrencyToken;
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
        await _dbContext.Entry(entity).Collection(e => e.Ingredients).LoadAsync(cancellationToken);
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

    private static ValueTask ValidateAddOrThrow(Pie entity, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }
    private static ValueTask ValidateUpdateOrThrow(Pie entity, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }
    private ValueTask<(bool allowed, string reason)> AllowsDelete(Guid id, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult((true, string.Empty));
    }
} 
