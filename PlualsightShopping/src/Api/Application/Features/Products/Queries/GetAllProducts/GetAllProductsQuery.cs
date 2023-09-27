using MediatR;
using PlualsightShopping.Shared.DataTransferObjects;

namespace PluralsightShopping.Api.Application.Features.Products.Queries.GetAllProducts;

public sealed record class GetAllProductsQuery() : IStreamRequest<ProductDto>;
