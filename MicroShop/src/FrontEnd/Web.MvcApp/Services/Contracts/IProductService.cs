using MicroShop.Web.MvcApp.Models;
using MicroShop.Web.MvcApp.Models.Products;

namespace MicroShop.Web.MvcApp.Services.Contracts;

public interface IProductService
{
    public Task<ResponseDto<ProductDto>?> GetProductById(int id);
    public Task<ResponseDto<IEnumerable<ProductDto>>?> GetProducts();
    public Task<ResponseDto<ProductDto>?> AddProduct(AddOrEditProductDto data);
    public Task<ResponseDto<ProductDto>?> EditProduct(int id, AddOrEditProductDto data);
    public Task<ResponseDto<ProductDto>?> DeleteProduct(int id);
}
