using MicroShop.Services.Products.App.Models;
using MicroShop.Services.Products.App.Services.Contracts;

namespace MicroShop.Services.Products.App.Services;

public class ImageFileService<TFileSystem>(IHostEnvironment environment) : IImageFileService
    where TFileSystem : IFileSystem
{
    private readonly string _contentRootPath = environment.ContentRootPath;

    /// <inheritdoc />
    public async ValueTask AddImage(IFileStreamSource file, Product product, bool replaceExisting = true)
    {
        string? originalPath = product.ImageLocalPath;
        string path = product.ImageLocalPath is not { Length: > 0 }
            ? GenerateFilename(file.FileName)
            : GenerateFilename(TFileSystem.GetFileName(product.ImageLocalPath));

        if (!IsImageAllowed(path))
        {
            return;
        }
        EnsureImagesPathExist();
        RemoveFileIfExists(originalPath);

        await using Stream output = TFileSystem.OpenStream(path, FileMode.Create);
        await file.CopyToAsync(output);
        product.ImageLocalPath = path;
    }

    /// <inheritdoc />
    public ValueTask DeleteImage(Product product)
    {
        string? path = product.ImageLocalPath;
        if (path is not { Length: > 0 })
        {
            return ValueTask.CompletedTask;
        }
        if (!IsImageAllowed(path))
        {
            return ValueTask.CompletedTask;
        }

        if (TFileSystem.FileExists(path))
        {
            TFileSystem.FileDelete(path);
        }

        return ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    public string GetRelativeImagePath(string localImagePath) =>
        localImagePath.StartsWith(_contentRootPath, StringComparison.OrdinalIgnoreCase)
            ? localImagePath.Replace(_contentRootPath, string.Empty).TrimStart('\\', '/')
            : localImagePath;

    private void EnsureImagesPathExist()
    {
        string directory = Path.Combine(_contentRootPath, "Images");
        if (!TFileSystem.DirectoryExists(directory))
        {
            TFileSystem.CreateDirectory(directory);
        }
    }

    private string GenerateFilename(string providedFilename)
    {
        string extension = TFileSystem.GetExtension(providedFilename);
        return Path.Combine(_contentRootPath, "Images", $"{Guid.NewGuid():N}{extension}");
    }
    private bool IsImageAllowed(string path)
    {
        // if this gets bigger then move away from array approach, simple rule chain implementation
        Func<string, bool>[] validators = [IsFileInImagesFolder, IsExtensionAllowed];
        return validators.All(v => v(path));

    }
    private bool IsFileInImagesFolder(string path)
    {
        path = TFileSystem.GetFullPath(path);
        string imagesDirectory = Path.Combine(_contentRootPath, "Images");
        return path.StartsWith(imagesDirectory, StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsExtensionAllowed(string path)
    {
        string extension = TFileSystem.GetExtension(path).ToUpperInvariant();
        return extension is ".JPG" or ".PNG";
    }

    private static void RemoveFileIfExists(string? path)
    {
        if (path is { Length: > 0 } && TFileSystem.FileExists(path))
        {
            TFileSystem.FileDelete(path);
        }
    }
}
