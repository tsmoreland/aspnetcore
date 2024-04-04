using System.Collections.Concurrent;
using MicroShop.Services.Products.ProductsApiApp.Services.Contracts;
using Moq;

namespace MicroShop.Services.Products.ProductsApiApp.Tests.Services;

public sealed class MockFileSystem : IFileSystem
{
    private static readonly ConcurrentDictionary<Guid, Mock<IMockFileSystemInstance>> s_instanceById = new();
    private static readonly AsyncLocal<Guid> s_id = new();

    public static void AddMock()
    {
        Guid id = Guid.NewGuid();
        s_instanceById.AddOrUpdate(id, Add, Update);
        s_id.Value = id;

        return;

        static Mock<IMockFileSystemInstance> Add(Guid id)
        {
            _ = id;
            return new Mock<IMockFileSystemInstance>();
        }
        static Mock<IMockFileSystemInstance> Update(Guid id, Mock<IMockFileSystemInstance> existingInstance)
        {
            _ = id;
            _ = existingInstance;
            return new Mock<IMockFileSystemInstance>();
        }
    }

    public static Mock<IMockFileSystemInstance> Mock =>
        s_instanceById.TryGetValue(s_id.Value, out Mock<IMockFileSystemInstance>? mock)
            ? mock
            : throw new KeyNotFoundException("No mock added for this async thread");

    private static IMockFileSystemInstance Instance => Mock.Object;

    /// <inheritdoc />
    public static bool FileExists(string? path) => Instance.FileExists(path);

    /// <inheritdoc />
    public static void FileDelete(string path) => Instance.FileDelete(path);

    /// <inheritdoc />
    public static Stream OpenStream(string path, FileMode mode) => Instance.OpenStream(path, mode);

    /// <inheritdoc />
    public static bool DirectoryExists(string? path) => Instance.DirectoryExists(path);

    /// <inheritdoc />
    public static void CreateDirectory(string path) => Instance.CreateDirectory(path);

    /// <inheritdoc />
    public static string GetExtension(string filename) => Instance.GetExtension(filename);

    /// <inheritdoc />
    public static string GetFullPath(string path) => Instance.GetFullPath(path);

    /// <inheritdoc />
    public static string? GetFileName(string? path) => Instance.GetFileName(path);
}

public interface IMockFileSystemInstance
{
    public bool FileExists(string? path);

    public void FileDelete(string path);

    public Stream OpenStream(string path, FileMode mode);

    public bool DirectoryExists(string? path);

    public void CreateDirectory(string path);

    public string GetExtension(string filename);

    public string GetFullPath(string path);

    public string? GetFileName(string? path);
}
