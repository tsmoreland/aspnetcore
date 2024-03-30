using System.Runtime.CompilerServices;
using MicroShop.Services.Products.ProductsApiApp.Services.Contracts;

namespace MicroShop.Services.Products.ProductsApiApp.Services;

public sealed class FormFileStreamSource(IFormFile file) : IFileStreamSource
{
    /// <inheritdoc />
    public string FileName => file.FileName;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Stream OpenReadStream() => file.OpenReadStream();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(Stream target) => file.CopyTo(target);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default) => file.CopyToAsync(target, cancellationToken);

    public static FormFileStreamSource FromFormFile(IFormFile file) => new(file);
}
