using MicroShop.Services.ShoppingCart.ApiApp.Infrastructure.Data;
using MicroShop.Services.ShoppingCart.ApiApp.Models;
using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects;
using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects.Request;
using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects.Response;
using MicroShop.Services.ShoppingCart.ApiApp.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace MicroShop.Services.ShoppingCart.ApiApp.Services;

public sealed class CartService(AppDbContext dbContext, ILogger<CartService> logger) : ICartService
{
    /// <inheritdoc/>
    public async Task<ResponseDto<CartSummaryDto>> Upsert(string userId, UpsertCartDto item, CancellationToken cancellationToken = default)
    {
        try
        {
            CartHeader? header = item.HeaderId is not null
                ? await dbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.Id == item.HeaderId && e.UserId == userId, default)
                : null;
            if (header is null)
            {
                return await CreateCart(userId, item, cancellationToken);
            }
            else
            {
                return await UpdateCart(header.Id, userId, item, cancellationToken);
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
            .Select(e => e.HeaderId).FirstOrDefaultAsync(cancellationToken);
        int count = await dbContext.CartDetails
            .Include(e => e.Header)
            .Where(e => e.Id == cartDetailsId && e.Header.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);

        logger.LogInformation("Deleted {Count} one details entry of {CartDetailsId} for {UserId}", count, cartDetailsId, userId);

        if (await dbContext.CartDetails.Include(e => e.Header)
                .CountAsync(e => e.HeaderId == headerId && e.Header.UserId == userId, cancellationToken) != 0)
        {
            return ResponseDto.Ok();
        }

        count = await dbContext.CartHeaders
            .Where(e => e.Id == headerId && e.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);
        logger.LogInformation("Deleted {Count} one cart {CartHeaderId} for {UserId}", count, headerId, userId);
        return ResponseDto.Ok();
    }

    /// <inheritdoc />
    public async Task<ResponseDto<CartSummaryDto>> GetByUserId(string userId, CancellationToken cancellationToken = default)
    {
        CartHeader? header = await dbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.UserId == userId, cancellationToken);
        if (header is null)
        {
            return ResponseDto.Error<CartSummaryDto>("No cart found for user");
        }

        List<CartItemDto> items = await dbContext.CartDetails.AsNoTracking()
            .Include(e => e.Header)
            .Where(e => e.Header.UserId == userId && e.HeaderId == header.Id)
            .Select(e => new { e.Id, e.ProductId })
            .AsAsyncEnumerable()
            .Select(p => new CartItemDto(p.Id, p.ProductId, string.Empty, 0.0, null))
            .ToListAsync(cancellationToken);

        // TODO: using product api get price, name and image url

        const double cartTotal = 0.0; // calculate from items
        CartSummaryDto summary = new(header.Id, header.CouponCode, header.Discount, cartTotal, items); 
        return ResponseDto.Ok(summary);
    }

    private async Task<ResponseDto<CartSummaryDto>> UpdateCart(int cartHeaderId, string userId, UpsertCartDto item, CancellationToken cancellationToken)
    {
        CartDetails? existingItem = await dbContext.CartDetails.Include(e => e.Header)
            .FirstOrDefaultAsync(e => e.ProductId == item.ProductId && e.Header.UserId == userId, cancellationToken);
        if (existingItem is null)
        {
            CartDetails details = new(cartHeaderId, item.ProductId);
            dbContext.CartDetails.Add(details);
            await dbContext.SaveChangesAsync(cancellationToken);

            // need to retrieve header for coupon code and discount, and to recalculate cart total (db func?)
        }
        else
        {
            existingItem.Count = item.Count;
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return ResponseDto.Ok(new CartSummaryDto(cartHeaderId, null, 0.0, 0.0, []));
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
}
