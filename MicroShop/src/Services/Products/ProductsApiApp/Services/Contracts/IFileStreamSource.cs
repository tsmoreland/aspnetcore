namespace MicroShop.Services.Products.ProductsApiApp.Services.Contracts;

public interface IFileStreamSource
{
    /// <summary>
    /// Gets the file name from the Content-Disposition header.
    /// </summary>
    string FileName { get; }

    /// <summary>
    /// Opens the request stream for reading the uploaded file.
    /// </summary>
    Stream OpenReadStream();

    /// <summary>
    /// Copies the contents of the uploaded file to the <paramref name="target" /> stream.
    /// </summary>
    /// <param name="target">The stream to copy the file contents to.</param>
    void CopyTo(Stream target);

    /// <summary>
    /// Asynchronously copies the contents of the uploaded file to the <paramref name="target" /> stream.
    /// </summary>
    /// <param name="target">The stream to copy the file contents to.</param>
    /// <param name="cancellationToken"></param>
    Task CopyToAsync(Stream target, CancellationToken cancellationToken = default (CancellationToken));
}
