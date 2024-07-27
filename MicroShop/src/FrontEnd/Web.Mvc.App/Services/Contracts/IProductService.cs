using MicroShop.Web.Mvc.App.Models;
using MicroShop.Web.Mvc.App.Models.Products;

namespace MicroShop.Web.Mvc.App.Services.Contracts;

public interface IProductService
{
    public Task<ResponseDto<ProductDto>?> GetProductById(int id);
    public Task<ResponseDto<IEnumerable<ProductDto>>?> GetProductsByCategoryName(string categoryName);
    public Task<ResponseDto<IEnumerable<ProductDto>>?> GetProducts();
    public Task<ResponseDto<ProductDto>?> AddProduct(AddProductDto data);
    public Task<ResponseDto<ProductDto>?> UpdateProduct(ProductDto data);
    public Task<ResponseDto<ProductDto>?> DeleteProduct(int id);
}
