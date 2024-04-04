using FluentAssertions;
using MicroShop.Services.Products.ProductsApiApp.Models;
using MicroShop.Services.Products.ProductsApiApp.Services;
using MicroShop.Services.Products.ProductsApiApp.Services.Contracts;
using Microsoft.Extensions.Hosting;
using Moq;

namespace MicroShop.Services.Products.ProductsApiApp.Tests.Services;

public sealed class ImageFileServiceTests
{
    private readonly Mock<IHostEnvironment> _environment = new();
    private const string ContentRoot = @"T:\wwwroot";

    public ImageFileServiceTests()
    {
        _environment.SetupGet(m => m.ContentRootPath).Returns(ContentRoot);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task AddImage_ShouldGenerateFilename_WhenImageLocalPathIsNull(bool replaceExisting)
    {
        (Product product, Mock<IFileStreamSource> file) = LocalArrangeAddImage();
        string fullpath = string.Empty;
        MockFileSystem.Mock
            .Setup(m => m.GetFullPath(It.IsAny<string>()))
            .Callback((string path) => fullpath = path)
            .Returns(() => fullpath);

        ImageFileService<MockFileSystem> service = new(_environment.Object);

        await service.AddImage(file.Object, product, replaceExisting);

        Assert.Multiple(
            () => product.ImageLocalPath.Should().NotBeNull(),
            () =>  product.ImageLocalPath.Should().Be(fullpath));

        return;

        static (Product Product, Mock<IFileStreamSource> File) LocalArrangeAddImage()
        {
            MockFileSystem.AddMock();
            Product product = new(1, "Orange Juice", 42.0, null, "Beverages", null, null);
            MockFileSystem.Mock.Setup(m => m.GetExtension(It.IsAny<string>())).Returns(".jpg");
            Mock<IFileStreamSource> file = new();
            file.Setup(m => m.OpenReadStream()).Returns(new MemoryStream());
            file.SetupGet(m => m.FileName).Returns("image.jpg");
            return (product, file);
        }
    }


    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task AddImage_ShouldNotGenerateFilename_WhenImageLocalPathIsNotNull(bool replaceExisting)
    {
        (string? filename, Product product, Mock<IFileStreamSource> file) = ArrangeAddImage(filename: "upload.jpg");
        ImageFileService<MockFileSystem> service = new(_environment.Object);

        await service.AddImage(file.Object, product, replaceExisting);
        Assert.Multiple(
            () => product.ImageLocalPath.Should().NotBeNull(),
            () => product.ImageLocalPath.Should().NotBe(filename),
            () => product.ImageLocalPath.Should().EndWith(".jpg"),
            () => product.ImageLocalPath.Should().StartWith($@"{ContentRoot}\Images\"));
    }

    [Fact]
    public async Task AddImage_ShouldDeleteImage_WhenImageExistsAndReplaceIsTrue()
    {
        (_, Product product, Mock<IFileStreamSource> file) = ArrangeAddImage();
        ImageFileService<MockFileSystem> service = new(_environment.Object);

        await service.AddImage(file.Object, product, true);

        MockFileSystem.Mock.Verify();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task AddImage_ShouldNotAddImage_WhenImageExtensionIsNotPngOrJpg(bool replaceExisting)
    {
        (_, Product product, Mock<IFileStreamSource> file) = ArrangeAddImage(filename: "upload.exe", setImageLocalPath: false);
        ImageFileService<MockFileSystem> service = new(_environment.Object);

        await service.AddImage(file.Object, product, replaceExisting);

        Assert.Multiple(
            () => product.ImageLocalPath.Should().BeNull(),
            () => MockFileSystem.Mock.Verify(m => m.OpenStream(It.IsAny<string>(), It.IsAny<FileMode>()), Times.Never));
    }

    private static (string? Filename, Product Product, Mock<IFileStreamSource> File) ArrangeAddImage(string? filename = "upload.jpg", bool setImageLocalPath = true)
    {
        string filenamePath = $@"{ContentRoot}\Images\{filename}";
        MockFileSystem.AddMock();
        Product product = new(1, "Orange Juice", 42.0, null, "Beverages", null, setImageLocalPath ? filenamePath : null);
        string fullpath = string.Empty;
        MockFileSystem.Mock
            .Setup(m => m.GetFullPath(It.IsAny<string>()))
            .Callback((string path) => fullpath = path)
            .Returns(() => fullpath);
        MockFileSystem.Mock.Setup(m => m.FileDelete(It.IsAny<string>())).Verifiable(Times.Once);
        MockFileSystem.Mock.Setup(m => m.FileExists(It.IsAny<string>())).Returns(true);
        MockFileSystem.Mock.Setup(m => m.GetExtension(It.IsAny<string>())).Returns(Path.GetExtension(filename) ?? ".jpg");
        MockFileSystem.Mock.Setup(m => m.GetFileName(It.IsAny<string>())).Returns(() => fullpath is { Length: 0 } ? filename : "???");
        Mock<IFileStreamSource> file = new();
        file.Setup(m => m.OpenReadStream()).Returns(new MemoryStream());
        file.SetupGet(m => m.FileName).Returns(() => filename ?? "upload.jpg");

        return (filenamePath, product, file);
    }

    [Fact]
    public async Task DeleteImage_ShouldDeleteFile_WhenImageLocalPathIsNonNullAndFileExists()
    {
        (Product product, _) = ArrangeDeleteImage("deleteMe.jpg", true);
        ImageFileService<MockFileSystem> service = new(_environment.Object);

        await service.DeleteImage(product);

        MockFileSystem.Mock.Verify();
    }

    private static (Product Product, Mock<IFileStreamSource> File) ArrangeDeleteImage(string? filename, bool fileExists, bool setImageLocalPath = true, bool expectDelete = true)
    {
        string? filenamePath = filename is not null
            ? $@"{ContentRoot}\Images\{filename}"
            : null;
        MockFileSystem.AddMock();

        MockFileSystem.Mock.Setup(m => m.FileExists(It.IsAny<string>())).Returns(fileExists && filename is { Length: > 0 } && setImageLocalPath);
        if (expectDelete && fileExists) // can't expect a delete when the file doesn't exist.
        {
            MockFileSystem.Mock.Setup(m => m.FileDelete(It.IsAny<string>())).Verifiable(Times.Once);
        }
        else
        {
            MockFileSystem.Mock.Setup(m => m.FileDelete(It.IsAny<string>())).Verifiable(Times.Never);
        }

        string fullpath = string.Empty;
        MockFileSystem.Mock
            .Setup(m => m.GetFullPath(It.IsAny<string>()))
            .Callback((string path) => fullpath = path)
            .Returns(() => fullpath);
        MockFileSystem.Mock.Setup(m => m.GetExtension(It.IsAny<string>())).Returns(Path.GetExtension(filename) ?? ".jpg");

        Product product = new(1, "Apple Juice", 24.0, null, "Beverages", null, setImageLocalPath ? filenamePath : null);

        Mock<IFileStreamSource> file = new();

        return (product, file);
    }
}
