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
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.Admin.Infrastructure.Persistence.Repositories;

[ReadOnlyRepository("BethanysPieShop.Admin.Domain.Models.Category", "BethanysPieShop.Admin.Domain.Projections.CategorySummary")]
[WritableRepository("BethanysPieShop.Admin.Domain.Models.Category")]
public sealed partial class CategoryRepository : ICategoryRepository
{
    private readonly AdminDbContext _dbContext;
    private DbSet<Category> Entities => _dbContext.Categories;

    public CategoryRepository(AdminDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public partial IAsyncEnumerable<Category> GetAll();

    /// <inheritdoc />
    public partial IAsyncEnumerable<CategorySummary> GetSummaries();

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
}
