using MicroShop.Services.ShoppingCart.App.Models.DataTransferObjects;

namespace MicroShop.Services.ShoppingCart.App.Services.Contracts;

public interface IProductService
{
    public IAsyncEnumerable<ProductDto> GetProductsInIds(IEnumerable<int> ids, CancellationToken cancellationToken = default);
}
