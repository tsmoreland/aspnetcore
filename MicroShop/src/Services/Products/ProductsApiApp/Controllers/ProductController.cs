using System.Net.Mime;
using MicroShop.Services.Products.ProductsApiApp.Infrastructure.Data;
using MicroShop.Services.Products.ProductsApiApp.Models;
using MicroShop.Services.Products.ProductsApiApp.Models.DataTransferObjects;
using MicroShop.Services.Products.ProductsApiApp.Services;
using MicroShop.Services.Products.ProductsApiApp.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroShop.Services.Products.ProductsApiApp.Controllers;

[Route("api/product")]
[ApiController]
public class ProductController(AppDbContext dbContext, IImageFileService fileService, IWebHostEnvironment environment,
    ILogger<ProductController> logger) : ControllerBase
{
    [HttpPost]
    [Authorize("ADMIN")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseDto<ProductDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseDto<ProductDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ResponseDto<ProductDto>>> AddProduct([FromForm] AddOrEditProductDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ResponseDto.Error<ProductDto>("One or more properties of the provided data are invalid"));
        }

        try
        {
            string? originalFilename = model.Image?.FileName;
            Product product = model.ToNewProduct();

            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();

            // TODO: tidy this up and shift a lot of it out of the controller (or in other cases out of the minimal api methods
            if (model.Image is not null)
            {
                string contentFolder = Path.Combine(environment.WebRootPath, "products");
                string filename = $"{product.Id}.{Path.GetExtension(originalFilename)}";
                product.ImageLocalPath = Path.Combine(contentFolder, filename);

                await fileService.AddImage(new FormFileStreamSource(model.Image), product, true);

                string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                product.ImageUrl = $"{baseUrl}/products/{filename}";
            }
            else
            {
                product.ImageUrl = "https://placehold.co/600x400/blue/white";
            }

            dbContext.Products.Update(product);
            await dbContext.SaveChangesAsync();

            return Ok(product);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred");
            return BadRequest(ResponseDto.Error<ProductDto>("One or more properties of the provided data are invalid"));
        }
    }

    [HttpPut("{id:int}")]
    [Authorize("ADMIN")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseDto<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<ProductDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseDto<ProductDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ResponseDto<ProductDto>>> UpdateProduct(int id, [FromForm] AddOrEditProductDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ResponseDto.Error<ProductDto>("One or more properties of the provided data are invalid"));
        }

        try
        {
            Product? existing = dbContext.Products.FirstOrDefault(e => e.Id == id);
            if (existing is null)
            {
                return NotFound(ResponseDto.Error<ProductDto>("Product not found"));
            }

            // TODO: tidy this up and shift a lot of it out of the controller (or in other cases out of the minimal api methods
            existing.Name = model.Name;
            existing.Price = model.Price;
            existing.Description = model.Description;
            existing.CategoryName = model.CategoryName;
            existing.ImageUrl = model.ImageUrl;

            if (model.Image is not null)
            {
                await fileService.AddImage(new FormFileStreamSource(model.Image), existing, true);
            }

            await dbContext.SaveChangesAsync();

            ProductDto result = new(existing);
            return Ok(ResponseDto.Ok(result));

        }
        catch (Exception)
        {
            // cheap out and blame the client for now, more precise exception handling would handle this better
            return BadRequest(ResponseDto.Error<ProductDto>("One or more properties of the provided data are invalid"));
        }
    }
}
