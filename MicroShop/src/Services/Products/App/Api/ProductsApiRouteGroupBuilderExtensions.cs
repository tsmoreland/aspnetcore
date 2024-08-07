﻿using MicroShop.Services.Products.App.Infrastructure.Data;
using MicroShop.Services.Products.App.Models;
using MicroShop.Services.Products.App.Models.DataTransferObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.Products.App.Api;

internal static class ProductsApiRouteGroupBuilderExtensions
{
    public static RouteGroupBuilder MapProductsApi(this RouteGroupBuilder builder)
    {
        // Map Add and Update aren't binding correctly at present, both would need [FromForm] for multi-part/form-data to handle the image
        // but that isn't binding correctly due to missing FormDataConverter, as of yet I can see no way to add a custom one to handle it
        return builder
            //.MapAddProduct()
            .MapGetAllProducts()
            .MapGetProductById()
            .MapGetProductsByCategory()
            .MapGetProductsInIds()
            //.MapUpdateProduct()
            .MapDeleteProduct()
            .WithTags("Product");
    }

#if USE_MINIMAL_ADD_UPDATE
#pragma warning disable IDE0051 // Remove unused private members
    private static RouteGroupBuilder MapAddProduct(this RouteGroupBuilder builder)
#pragma warning restore IDE0051 // Remove unused private members
    {
        builder
            .MapPost("/", async ([FromForm] AddOrEditProductDto data, [FromServices] AppDbContext dbContext) =>
            {
                try
                {
                    Product product = data.ToNewProduct();
                    dbContext.Products.Add(product);
                    await dbContext.SaveChangesAsync();

                    ProductDto result = new(product);
                    return Results.Created((string?)null, ResponseDto.Ok(result));
                }
                catch (Exception)
                {
                    // cheap out and blame the client for now, more precise exception handling would handle this better
                    return Results.BadRequest(ResponseDto.Error<ProductDto>("One or more properties of the provided data are invalid"));
                }
            })
            .RequireAuthorization("ADMIN")
            .WithName("AddProduct")
            .WithOpenApi();
        return builder;
    }
#endif

    private static RouteGroupBuilder MapGetAllProducts(this RouteGroupBuilder builder)
    {
        builder
            .MapGet("/", async ([FromServices] AppDbContext dbContext) =>
                Results.Ok(new ResponseDto<IEnumerable<ProductDto>>(await dbContext.Products.AsNoTracking()
                    .Select(c => new ProductDto(c))
                    .ToListAsync())))
            .WithName("GetAllProducts")
            .WithOpenApi();
        return builder;
    }

    private static RouteGroupBuilder MapGetProductsByCategory(this RouteGroupBuilder builder)
    {
        builder
            .MapGet("/{catagory}", Handler)
            .WithName("GetProductsByCategory")
            .WithOpenApi();

        return builder;

        async Task<Ok<ResponseDto<IEnumerable<ProductDto>>>> Handler([FromRoute] string catagory, [FromServices] AppDbContext dbContext)
        {
            string normalizedCategory = catagory.ToUpperInvariant();
            IEnumerable<ProductDto> products = await dbContext.Products.AsNoTracking()
                .Where(e => e.NormalizedCategoryName == normalizedCategory)
                .Select(c => new ProductDto(c))
                .ToListAsync();
            return TypedResults.Ok(ResponseDto.Ok(products));
        }
    }

    private static RouteGroupBuilder MapGetProductById(this RouteGroupBuilder builder)
    {
        builder
            .MapGet("/{id:int}", Handler)
            .WithName("GetProductById")
            .WithOpenApi();
        return builder;

        async Task<Results<Ok<ResponseDto<ProductDto>>, NotFound<ResponseDto<ProductDto>>>> Handler([FromServices] AppDbContext dbContext, [FromRoute] int id)
        {
            ProductDto? dto = await dbContext.Products.AsNoTracking()
                .Where(e => e.Id == id)
                .Select(c => new ProductDto(c))
                .AsAsyncEnumerable()
                .FirstOrDefaultAsync();
            return dto is not null
                ? TypedResults.Ok(ResponseDto.Ok(dto))
                : TypedResults.NotFound(ResponseDto.Error<ProductDto>("Product matching id not found"));
        }
    }

    private static RouteGroupBuilder MapGetProductsInIds(this RouteGroupBuilder builder)
    {
        builder
            .MapPost("/in", Handler)
            .WithName("GetProductsInIds")
            .WithOpenApi();

        return builder;

        Results<Ok<IAsyncEnumerable<ProductDto>>, BadRequest<IAsyncEnumerable<ProductDto>>> Handler([FromServices] AppDbContext dbContext, [FromBody] IReadOnlyList<int> ids)
        {
            if (ids.Count == 0)
            {
                return TypedResults.BadRequest(AsyncEnumerable.Empty<ProductDto>());
            }
            IAsyncEnumerable<ProductDto> products = dbContext.Products
                .Where(p => ids.Contains(p.Id))
                .Select(p => new ProductDto(p))
                .AsAsyncEnumerable();
            return TypedResults.Ok(products);
        }
    }

#if USE_MINIMAL_ADD_UPDATE
#pragma warning disable IDE0051 // Remove unused private members
    private static RouteGroupBuilder MapUpdateProduct(this RouteGroupBuilder builder)
#pragma warning restore IDE0051 // Remove unused private members
    {
        builder
            .MapPut("/{id:int}", async ([FromRoute] int id, [FromForm] AddOrEditProductDto data, [FromServices] AppDbContext dbContext) =>
            {
                try
                {
                    Product product = data.ToProduct(id);
                    dbContext.Products.Update(product);
                    await dbContext.SaveChangesAsync();

                    ProductDto result = new(product);
                    return Results.Ok(ResponseDto.Ok(result));

                }
                catch (Exception)
                {
                    // cheap out and blame the client for now, more precise exception handling would handle this better
                    return Results.BadRequest(ResponseDto.Error<ProductDto>("One or more properties of the provided data are invalid"));
                }
            })
            .RequireAuthorization("ADMIN")
            .WithName("UpdateProduct")
            .WithOpenApi();
        return builder;
    }
#endif

    private static RouteGroupBuilder MapDeleteProduct(this RouteGroupBuilder builder)
    {
        builder
            .MapDelete("/{id:int}", async ([FromRoute] int id, [FromServices] AppDbContext dbContext, [FromServices] IWebHostEnvironment environment) =>
            {
                try
                {
                    Product? product = await dbContext.Products.FindAsync([id]);
                    if (product is not null)
                    {
                        if (product.ImageLocalPath is not { Length: > 0 } && File.Exists(product.ImageLocalPath))
                        {
                            File.Delete(product.ImageLocalPath);
                        }

                        dbContext.Products.Remove(product);
                        await dbContext.SaveChangesAsync();
                    }

                    return NoContentWithResponseResult.Success();
                }
                catch (Exception)
                {
                    // cheap out and blame the client for now, more precise exception handling would handle this better
                    return Results.BadRequest(ResponseDto.Error<ProductDto>("One or more properties of the provided data are invalid"));
                }
            })
            .RequireAuthorization("ADMIN")
            .WithName("DeleteProduct")
            .WithOpenApi();

        return builder;
    }
}
