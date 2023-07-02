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
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TennisByTheSea.Domain.Contracts.Products.Queries;

namespace TennisByTheSea.Infrastructure.ExternalApi.Products.Queries;

public sealed class GetProductsQueryHandler : IStreamRequestHandler<GetProductsQuery, Product>
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<GetProductsQueryHandler> _logger;
    private readonly HttpClient _client;
    private const string CacheKey = "Products";

    public GetProductsQueryHandler(
        IHttpClientFactory httpClientFactory,
        IMemoryCache cache,
        ILogger<GetProductsQueryHandler> logger)
    {
        _cache = cache;
        _logger = logger;
        _client = httpClientFactory.CreateClient("Products");
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<Product> Handle(GetProductsQuery request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (_cache.TryGetValue(CacheKey, out IReadOnlyList<Product>? products) && products is not null)
        {
            foreach (Product product in products)
            {
                yield return product;
            }
        }

        List<Product> productsForCache = new();
        IAsyncEnumerable<Product> productsFromApi = GetProductsFromApi(cancellationToken);
        await foreach (Product product in productsFromApi.WithCancellation(cancellationToken))
        {
            productsForCache.Add(product);
            yield return product;
        }
        _cache.Set(CacheKey, productsForCache.AsReadOnly(), GetCacheOptions());
    }

    /// <summary>
    /// If this were web assembly then we'd use something like this to ensure we can deserialize as a stream
    /// <code>
    /// HttpRequestMessage request = new(HttpMethod.Get, "api/products");
    /// request.SetBrowserResponseStreamingEnabled(true);
    /// HttpResponseMessage response = await _client.SendAsync(request, cancellationToken);
    /// </code>
    /// </summary>
    private async IAsyncEnumerable<Product> GetProductsFromApi([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IAsyncEnumerable<Product?>? products = null;
        try
        {
            HttpResponseMessage response = await _client.GetAsync("api/products", cancellationToken);
            response.EnsureSuccessStatusCode();

            await using Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            products = JsonSerializer
                .DeserializeAsyncEnumerable<Product>(stream, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred attempting to retrieve products");
        }

        if (products is null)
        {
            yield break;
        }

        await foreach (Product? product in products.WithCancellation(cancellationToken))
        {
            if (product is not null)
            {
                yield return product;
            }
        }
    }

    private static MemoryCacheEntryOptions GetCacheOptions()
    {
        return new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
        };
    }
}
