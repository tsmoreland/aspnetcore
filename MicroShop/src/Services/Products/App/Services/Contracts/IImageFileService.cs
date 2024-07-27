using MicroShop.Services.Products.App.Models;

namespace MicroShop.Services.Products.App.Services.Contracts;

public interface IImageFileService
{
    ValueTask AddImage(IFileStreamSource file, Product product, bool replaceExisting = true);
    ValueTask DeleteImage(Product product);

    /// <summary>
    /// Returns the image path relative to content root
    /// </summary>
    string GetRelativeImagePath(string localImagePath);
}
