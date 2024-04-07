using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects;
using MicroShop.Services.ShoppingCart.ApiApp.Services.Contracts;

namespace MicroShop.Services.ShoppingCart.ApiApp.Services;

public sealed class ProductService(IHttpClientFactory clientFactory, ILogger<ProductService> logger) : IProductService
{
    /// <inheritdoc />
    public async IAsyncEnumerable<ProductDto> GetProductsInIds(IEnumerable<int> ids, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        IEnumerable<ProductDto> products;
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

            ResponseDto<IEnumerable<ProductDto>>? responseData = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<ProductDto>>>(cancellationToken);
            if (responseData is not { Success: true, Data: not null })
            {
                logger.LogError("API responded with successful status but unsuccessful result: {ErrorMessage}", responseData?.ErrorMessage ?? "Unknown Error");
                yield break;
            }
            products = responseData.Data;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving products: {ErrorMessage}", ex.Message);
            yield break;

        }

        foreach (ProductDto productDto in products)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                yield break;
            }
            yield return productDto;
        }
    }

    private HttpClient CreateClient() =>
        clientFactory.CreateClient("ProductsApi");
}
