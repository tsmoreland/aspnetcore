using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using MicroShop.Services.ShoppingCart.App.Models.DataTransferObjects;
using MicroShop.Services.ShoppingCart.App.Services.Contracts;

namespace MicroShop.Services.ShoppingCart.App.Services;

public sealed class ProductService(IHttpClientFactory clientFactory, ILogger<ProductService> logger) : IProductService
{
    /// <inheritdoc />
    public async IAsyncEnumerable<ProductDto> GetProductsInIds(IEnumerable<int> ids, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        IAsyncEnumerable<ProductDto>? collection = null;
        try
        {
            ReadOnlyCollection<int> idCollection = ids.ToList().AsReadOnly();
            if (idCollection.Count == 0)
            {
                yield break;
            }

            HttpResponseMessage response = await CreateClient().PostAsJsonAsync("/api/products/in", idCollection, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("failed to get products, response code was {StatusCode}", response.StatusCode);
                yield break;
            }

            collection = await response.Content.ReadFromJsonAsync<IAsyncEnumerable<ProductDto>>(cancellationToken);
            if (collection is null)
            {
                logger.LogError("API responded with empty collection");
                yield break;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving products: {ErrorMessage}", ex.Message);
            yield break;

        }

        await foreach (ProductDto product in collection)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                yield break;
            }
            yield return product;
        }
    }

    private HttpClient CreateClient() =>
        clientFactory.CreateClient("ProductsApi");
}
