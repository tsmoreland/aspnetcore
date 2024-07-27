using MicroShop.Web.Mvc.App.Models;
using MicroShop.Web.Mvc.App.Models.Products;
using MicroShop.Web.Mvc.App.Services.Contracts;

namespace MicroShop.Web.Mvc.App.Services;

public sealed class ProductService(IBaseService baseService) : IProductService
{
    private const string ClientName = "ProductsApi";

    /// <inheritdoc />
    public async Task<ResponseDto<ProductDto>?> GetProductById(int id) =>
        await SendAsync<ProductDto>(new RequestDto($"/api/products/{id}", null));

    /// <inheritdoc />
    public async Task<ResponseDto<IEnumerable<ProductDto>>?> GetProductsByCategoryName(string categoryName) =>
        await SendAsync<IEnumerable<ProductDto>>(new RequestDto($"/api/products/{categoryName}", null));

    /// <inheritdoc />
    public async Task<ResponseDto<IEnumerable<ProductDto>>?> GetProducts() =>
        await SendAsync<IEnumerable<ProductDto>>(new RequestDto("/api/products", null));

    /// <inheritdoc />
    public async Task<ResponseDto<ProductDto>?> AddProduct(AddProductDto data) =>
        await SendAsync<ProductDto>(
            new RequestDto(ApiType.Post, "/api/products", data),
            SupportedContentType.MultiPartForm);

    /// <inheritdoc />
    public async Task<ResponseDto<ProductDto>?> UpdateProduct(ProductDto data)
    {
        AddProductDto editData = new(data.Name, data.Price, data.Description, data.CategoryName, data.ImageUrl);
        return await SendAsync<ProductDto>(new RequestDto(ApiType.Put, $"/api/products/{data.Id}", editData),
            SupportedContentType.MultiPartForm);
    }

    /// <inheritdoc />
    public async Task<ResponseDto<ProductDto>?> DeleteProduct(int id) =>
        await SendAsync<ProductDto>(new RequestDto(ApiType.Delete, $"/api/products/{id}", null));

    private Task<ResponseDto<T>?> SendAsync<T>(
        RequestDto data,
        SupportedContentType contentType = SupportedContentType.ApplicationJson) =>
        baseService.SendAsync<T>(ClientName, data, contentType);
}
