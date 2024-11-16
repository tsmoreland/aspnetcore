using System.Runtime.CompilerServices;
using System.Text;
using BethanysPieShop.Admin.Domain.Contracts;
using BethanysPieShop.Admin.Domain.Models;
using BethanysPieShop.Admin.Domain.Projections;
using BethanysPieShop.Admin.Domain.ValueObjects;
using Microsoft.Extensions.Caching.Memory;

namespace BethanysPieShop.Admin.Infrastructure.Persistence.CachedRepositories;

public sealed class CachedCategoryRepository(ICategoryRepository repository, IMemoryCache cache) : ICategoryRepository
{
    // this won't work because the repository is scoped, may need a cookie to store these values then we can use that along with a cache id
    // failing that see if we can clear cache entries on partial name match - no good on that, maybe a limited range of cache ids? there are only so many sort / descending options
    private SortOrder? _lastOrderForAll;


    /// <inheritdoc />
    public async IAsyncEnumerable<Category> GetAll(CategoriesOrder orderBy, bool descending)
    {
        await Task.CompletedTask.ConfigureAwait(false);
        var cacheName = GetCacheName(ToArgs(orderBy, descending));

        if (_lastOrderForAll?.Equals(orderBy, descending) == true && cache.TryGetValue(cacheName, out IReadOnlyList<Category>? categories) && categories is not null)
        {
            foreach (var category in categories)
            {
                yield return category;
            }
        }

        categories = await repository.GetAll(orderBy, descending).ToListAsync().ConfigureAwait(false);
        var options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));
        cache.Set(cacheName, categories, options);
        _lastOrderForAll = new SortOrder(orderBy, descending);

        foreach (var category in categories)
        {
            yield return category;
        }
    }

    /// <inheritdoc />
    public IAsyncEnumerable<CategorySummary> GetSummaries(CategoriesOrder orderBy, bool descending)
    {
        return repository.GetSummaries(orderBy, descending);
    }

    /// <inheritdoc />
    public ValueTask<Page<CategorySummary>> GetSummaryPage(PageRequest<CategoriesOrder> request, CancellationToken cancellationToken)
    {
        return repository.GetSummaryPage(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task<Category?> FindById(Guid id, CancellationToken cancellationToken)
    {
        return repository.FindById(id, cancellationToken);
    }

    /// <inheritdoc />
    public async ValueTask Add(Category entity, CancellationToken cancellationToken)
    {
        await repository.Add(entity, cancellationToken).ConfigureAwait(false);
        ClearCache();
    }

    /// <inheritdoc />
    public async ValueTask Update(Category entity, CancellationToken cancellationToken)
    {
        await repository.Update(entity, cancellationToken).ConfigureAwait(false);
        ClearCache();
    }

    /// <inheritdoc />
    public void Delete(Category entity)
    {
        repository.Delete(entity);
        ClearCache();
    }

    /// <inheritdoc />
    public async ValueTask Delete(Guid id, CancellationToken cancellationToken = default)
    {
        await repository.Delete(id, cancellationToken).ConfigureAwait(false);
        ClearCache();
    }

    /// <inheritdoc />
    public ValueTask SaveChanges(CancellationToken cancellationToken)
    {
        return repository.SaveChanges(cancellationToken);
    }

    /// <inheritdoc />
    public async ValueTask Update(Guid id, string name, string? description, CancellationToken cancellationToken = default)
    {
        await repository.Update(id, name, description, cancellationToken).ConfigureAwait(false);
        ClearCache();
    }

    private static string GetCacheName(IEnumerable<object?> additionalArgs, [CallerMemberName] string memberName = "")
    {
        var cacheName = GetCacheName(memberName);
        StringBuilder builder = new();
        builder.Append(cacheName);
        foreach (var arg in additionalArgs.Where(a => a is not null))
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
        cache.Remove(GetCacheName(nameof(GetAll)));
        cache.Remove(GetCacheName(nameof(GetSummaries)));
        cache.Remove(GetCacheName(nameof(GetSummaryPage)));
    }

}
