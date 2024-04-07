using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects;

namespace MicroShop.Services.ShoppingCart.ApiApp.Services.Contracts;

public interface IProductService
{
    public IAsyncEnumerable<ProductDto> GetProductsInIds(IEnumerable<int> ids, CancellationToken cancellationToken = default);
}
