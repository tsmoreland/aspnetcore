using System.Diagnostics.CodeAnalysis;

namespace MicroShop.Services.Products.App.Services.Contracts;

public interface IFileSystem
{
    /// <inheritdoc cref="File.Exists(string?)"/>
    static abstract bool FileExists(string? path);

    /// <inheritdoc cref="File.Delete(string)"/>
    static abstract void FileDelete(string path);

    static abstract Stream OpenStream(string path, FileMode mode);

    /// <inheritdoc cref="Directory.Exists(string?)"/>
    static abstract bool DirectoryExists(string? path);

    /// <inheritdoc cref="Directory.CreateDirectory(string)"/>
    static abstract void CreateDirectory(string path);

    static abstract string GetExtension(string filename);

    /// <inheritdoc cref="Path.GetFullPath(string)"/>
    static abstract string GetFullPath(string path);

    /// <inheritdoc cref="Path.GetFileName(string?)"/>
    [return: NotNullIfNotNull(nameof(path))]
    static abstract string? GetFileName(string? path);
}
