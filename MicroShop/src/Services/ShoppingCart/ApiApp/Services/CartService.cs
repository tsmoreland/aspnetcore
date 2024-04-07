﻿using System.Collections.ObjectModel;
using MicroShop.Services.ShoppingCart.ApiApp.Infrastructure.Data;
using MicroShop.Services.ShoppingCart.ApiApp.Models;
using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects;
using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects.Request;
using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects.Response;
using MicroShop.Services.ShoppingCart.ApiApp.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace MicroShop.Services.ShoppingCart.ApiApp.Services;

public sealed class CartService(IProductService productService, AppDbContext dbContext, ILogger<CartService> logger) : ICartService
{
    /// <inheritdoc/>
    public async Task<ResponseDto<CartSummaryDto>> Upsert(string userId, UpsertCartDto item, CancellationToken cancellationToken = default)
    {
        try
        {
            CartHeader? header = item.HeaderId is not null
                ? await dbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.Id == item.HeaderId && e.UserId == userId, default).ConfigureAwait(false)
                : null;
            if (header is null)
            {
                return await CreateCart(userId, item, cancellationToken);
            }
            else
            {
                return await UpdateCart(header, userId, item, cancellationToken);
            }

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred processing this request");
            return ResponseDto.Error<CartSummaryDto>("An error occurred processing this request");
        }
    }

    /// <inheritdoc />
    public async Task<ResponseDto> RemoveFromCart(string userId, int cartDetailsId, CancellationToken cancellationToken = default)
    {
        int headerId = await dbContext.CartDetails
            .AsNoTracking()
            .Where(e => e.Id == cartDetailsId)
            .Select(e => e.HeaderId)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
        int count = await dbContext.CartDetails
            .Include(e => e.Header)
            .Where(e => e.Id == cartDetailsId && e.Header.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken)
            .ConfigureAwait(false);

        logger.LogInformation("Deleted {Count} one details entry of {CartDetailsId} for {UserId}", count, cartDetailsId, userId);

        if (await dbContext.CartDetails.Include(e => e.Header)
                .CountAsync(e => e.HeaderId == headerId && e.Header.UserId == userId, cancellationToken).ConfigureAwait(false) != 0)
        {
            return ResponseDto.Ok();
        }

        count = await dbContext.CartHeaders
            .Where(e => e.Id == headerId && e.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken)
            .ConfigureAwait(false);
        logger.LogInformation("Deleted {Count} one cart {CartHeaderId} for {UserId}", count, headerId, userId);
        return ResponseDto.Ok();
    }

    /// <inheritdoc />
    public async Task<ResponseDto<CartSummaryDto>> GetByUserId(string userId, CancellationToken cancellationToken = default)
    {
        CartHeader? header = await dbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.UserId == userId, cancellationToken).ConfigureAwait(false);
        if (header is null)
        {
            return ResponseDto.Error<CartSummaryDto>("No cart found for user");
        }

        List<CartItemDto> items = await GetCartItemsByUserId(userId, header.Id).ToListAsync(cancellationToken).ConfigureAwait(false);

        double cartTotal = await UpdateCartItemsAndCalculateCartTotal(items, cancellationToken);
        CartSummaryDto summary = new(header.Id, header.CouponCode, header.Discount, cartTotal, items); 
        return ResponseDto.Ok(summary);
    }

    private async Task<ResponseDto<CartSummaryDto>> UpdateCart(CartHeader header, string userId, UpsertCartDto item, CancellationToken cancellationToken)
    {
        CartDetails? existingItem = await dbContext.CartDetails.Include(e => e.Header)
            .FirstOrDefaultAsync(e => e.ProductId == item.ProductId && e.Header.UserId == userId, cancellationToken).ConfigureAwait(false);
        if (existingItem is null)
        {
            CartDetails details = new(header.Id, item.ProductId);
            dbContext.CartDetails.Add(details);
            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        else
        {
            existingItem.Count = item.Count;
            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        List<CartItemDto> items = await GetCartItemsByUserId(userId, header.Id).ToListAsync(cancellationToken).ConfigureAwait(false);
        double cartTotal = await UpdateCartItemsAndCalculateCartTotal(items, cancellationToken).ConfigureAwait(false);

        return ResponseDto.Ok(new CartSummaryDto(header.Id, header.CouponCode, header.Discount, cartTotal, items));
    }

    private async Task<ResponseDto<CartSummaryDto>> CreateCart(string userId, UpsertCartDto item, CancellationToken cancellationToken = default)
    {
        // calculate cost using product service
        CartHeader header = new(userId, null, 0.0, 0.0);

        await using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        dbContext.CartHeaders.Add(header);
        await dbContext.SaveChangesAsync(cancellationToken);

        // save header first because we want the id, may need to remove transaction for this
        CartDetails details = new(header, ProductDto.Empty(item.ProductId));
        dbContext.CartDetails.Add(details);
        await transaction.CommitAsync(cancellationToken);

        return ResponseDto.Ok(new CartSummaryDto(header.Id, header.CouponCode, header.Discount, 0.0, [details.ToCartItem()]));
    }
    private async Task<double> UpdateCartItemsAndCalculateCartTotal(List<CartItemDto> items, CancellationToken cancellationToken = default)
    {
        ReadOnlyDictionary<int, ProductDto> products = (await productService
            .GetProductsInIds(items.Select(i => i.ProductId), cancellationToken)
            .ToDictionaryAsync(p => p.Id, p => p, cancellationToken))
            .AsReadOnly();

        List<CartItemDto> missing = [];
        for (int i = 0; i < items.Count; i++)
        {
            if (!products.TryGetValue(items[i].ProductId, out ProductDto? product))
            {
                logger.LogWarning("Product matching {ProductId} not found", items[i].ProductId);
                missing.Add(items[i]);
                continue;
            }
            items[i] = items[i] with { ProductName = product.Name, Price = product.Price, ImageUrl = product.ImageUrl };
        }
        foreach (CartItemDto item in missing)
        {
            // TODO: should probably delete these
            items.Remove(item);
        }

        return items.Sum(i => i.Count * i.Price);
    }
    private IAsyncEnumerable<CartItemDto> GetCartItemsByUserId(string userId, int headerId)
    {
        return dbContext.CartDetails.AsNoTracking()
            .Include(e => e.Header)
            .Where(e => e.Header.UserId == userId && e.HeaderId == headerId)
            .Select(e => new { e.Id, e.ProductId, e.Count })
            .AsAsyncEnumerable()
            .Select(p => new CartItemDto(p.Id, p.ProductId, string.Empty, 0.0, null, p.Count));
    }

}
