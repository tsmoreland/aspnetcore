using MicroShop.Services.Products.ProductsApiApp.Services.Contracts;

namespace MicroShop.Services.Products.ProductsApiApp.Services;

public sealed class FileSystem : IFileSystem
{
    /// <inheritdoc />
    public static bool FileExists(string? path) => path is { Length: > 0 } && new FileInfo(path).Exists;

    /// <inheritdoc />
    public static void FileDelete(string path) => File.Delete(path);

    /// <inheritdoc />
    public static Stream OpenStream(string path, FileMode mode) => new FileStream(path, mode);

    /// <inheritdoc />
    public static bool DirectoryExists(string? path) => path is { Length: > 0 } && new DirectoryInfo(path).Exists;

    /// <inheritdoc />
    public static void CreateDirectory(string path) => Directory.CreateDirectory(path);

    /// <inheritdoc />
    public static string GetExtension(string filename) => new FileInfo(filename).Extension;

    /// <inheritdoc />
    public static string GetFullPath(string path) => Path.GetFullPath(path);
}
