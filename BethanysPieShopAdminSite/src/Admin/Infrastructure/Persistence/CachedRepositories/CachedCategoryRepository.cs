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

using System.Runtime.CompilerServices;
using System.Text;
using BethanysPieShop.Admin.Domain.Contracts;
using BethanysPieShop.Admin.Domain.Models;
using BethanysPieShop.Admin.Domain.Projections;
using BethanysPieShop.Admin.Domain.ValueObjects;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace BethanysPieShop.Admin.Infrastructure.Persistence.CachedRepositories;

public sealed class CachedCategoryRepository : ICategoryRepository
{
    private readonly ICategoryRepository _repository;
    private readonly IMemoryCache _cache;
    private SortOrder? _lastOrderForAll;

    public CachedCategoryRepository(ICategoryRepository repository, IMemoryCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<Category> GetAll(CategoriesOrder orderBy, bool descending)
    {
        await Task.CompletedTask;
        string cacheName = GetCacheName(ToArgs(orderBy, descending));

        if (_lastOrderForAll?.Equals(orderBy, descending) == true && _cache.TryGetValue(cacheName, out IReadOnlyList<Category>? categories) && categories is not null)
        {
            foreach (Category category in categories)
            {
                yield return category;
            }
        }

        categories = await _repository.GetAll(orderBy, descending).ToListAsync();
        MemoryCacheEntryOptions options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));
        _cache.Set(cacheName, categories, options);
        _lastOrderForAll = new SortOrder(orderBy, descending);

        foreach (Category category in categories)
        {
            yield return category;
        }
    }

    /// <inheritdoc />
    public IAsyncEnumerable<CategorySummary> GetSummaries(CategoriesOrder orderBy, bool descending)
    {
        return _repository.GetSummaries(orderBy, descending);
    }

    /// <inheritdoc />
    public ValueTask<Page<CategorySummary>> GetSummaryPage(PageRequest<CategoriesOrder> request, CancellationToken cancellationToken)
    {
        return _repository.GetSummaryPage(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task<Category?> FindById(Guid id, CancellationToken cancellationToken)
    {
        return _repository.FindById(id, cancellationToken);
    }

    /// <inheritdoc />
    public async ValueTask Add(Category entity, CancellationToken cancellationToken)
    {
        await _repository.Add(entity, cancellationToken);
        ClearCache();
    }

    /// <inheritdoc />
    public async ValueTask Update(Category entity, CancellationToken cancellationToken)
    {
        await _repository.Update(entity, cancellationToken);
        ClearCache();
    }

    /// <inheritdoc />
    public void Delete(Category entity)
    {
        _repository.Delete(entity);
        ClearCache();
    }

    /// <inheritdoc />
    public async ValueTask Delete(Guid id, CancellationToken cancellationToken = default)
    {
        await _repository.Delete(id, cancellationToken);
        ClearCache();
    }

    /// <inheritdoc />
    public ValueTask SaveChanges(CancellationToken cancellationToken)
    {
        return _repository.SaveChanges(cancellationToken);
    }

    /// <inheritdoc />
    public async ValueTask Update(Guid id, string name, string? description, CancellationToken cancellationToken = default)
    {
        await _repository.Update(id, name, description, cancellationToken);
        ClearCache();
    }

    private static string GetCacheName(IEnumerable<object?> additionalArgs, [CallerMemberName] string memberName = "")
    {
        string cacheName = GetCacheName(memberName);
        StringBuilder builder = new();
        builder.Append(cacheName);
        foreach (object? arg in additionalArgs.Where(a => a is not null))
        {
            builder.Append('.').Append(arg!);
        }
        return builder.ToString();
    }
    private static string GetCacheName([CallerMemberName] string memberName = "") =>
        $"{nameof(CachedCategoryRepository)}.{memberName}";
    private static IEnumerable<object?> ToArgs(params object?[] arguments) => arguments;

    private sealed record class SortOrder(CategoriesOrder OrderBy, bool Descending)
    {
        public bool Equals(CategoriesOrder orderBy, bool descending) => OrderBy == orderBy && Descending == descending;
    }
    private void ClearCache()
    {
        _cache.Remove(GetCacheName(nameof(GetAll)));
        _cache.Remove(GetCacheName(nameof(GetSummaries)));
        _cache.Remove(GetCacheName(nameof(GetSummaryPage)));
    }

}
