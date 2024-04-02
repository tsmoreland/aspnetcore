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
        //(_, Product product, Mock<IFileStreamSource> file) = ArrangeAddImage();
        MockFileSystem.AddMock();
        string fullpath = string.Empty;
        Product product = new(1, "Orange Juice", 42.0, null, "Beverages", null, null);
        MockFileSystem.Mock
            .Setup(m => m.GetFullPath(It.IsAny<string>()))
            .Callback((string path) => fullpath = path)
            .Returns(() => fullpath);
        MockFileSystem.Mock.Setup(m => m.GetExtension(It.IsAny<string>())).Returns(".jpg");
        Mock<IFileStreamSource> file = new();
        file.Setup(m => m.OpenReadStream()).Returns(new MemoryStream());
        file.SetupGet(m => m.FileName).Returns("image.jpg");

        ImageFileService<MockFileSystem> service = new(_environment.Object);

        await service.AddImage(file.Object, product, replaceExisting);

        Assert.Multiple(
            () => product.ImageLocalPath.Should().NotBeNull(),
            () =>  product.ImageLocalPath.Should().Be(fullpath));
    }


    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task AddImage_ShouldNotGenerateFilename_WhenImageLocalPathIsNotNull(bool replaceExisting)
    {
        (string? filename, Product product, Mock<IFileStreamSource> file) = ArrangeAddImage();
        /*
        const string filename = $@"{ContentRoot}\Images\upload.jpg";
        MockFileSystem.AddMock();
        Product product = new(1, "Orange Juice", 42.0, null, "Beverages", null, filename);
        string fullpath = string.Empty;
        MockFileSystem.Mock
            .Setup(m => m.GetFullPath(It.IsAny<string>()))
            .Callback((string path) => fullpath = path)
            .Returns(() => fullpath);
        MockFileSystem.Mock.Setup(m => m.GetExtension(It.IsAny<string>())).Returns(".jpg");
        Mock<IFileStreamSource> file = new();
        file.Setup(m => m.OpenReadStream()).Returns(new MemoryStream());
        file.SetupGet(m => m.FileName).Returns(filename);
        */
        ImageFileService<MockFileSystem> service = new(_environment.Object);

        await service.AddImage(file.Object, product, replaceExisting);
        Assert.Multiple(
            () => product.ImageLocalPath.Should().NotBeNull(),
            () =>  product.ImageLocalPath.Should().Be(filename));
    }

    [Fact]
    public async Task AddImage_ShouldDeleteImage_WhenImageExistsAndReplaceIsTrue()
    {
        (_, Product product, Mock<IFileStreamSource> file) = ArrangeAddImage();

        /*
        const string filename = $@"{ContentRoot}\Images\upload.jpg";
        MockFileSystem.AddMock();
        Product product = new(1, "Orange Juice", 42.0, null, "Beverages", null, filename);
        string fullpath = string.Empty;
        MockFileSystem.Mock
            .Setup(m => m.GetFullPath(It.IsAny<string>()))
            .Callback((string path) => fullpath = path)
            .Returns(() => fullpath);
        MockFileSystem.Mock.Setup(m => m.FileDelete(It.IsAny<string>())).Verifiable(Times.Once);
        MockFileSystem.Mock.Setup(m => m.FileExists(It.IsAny<string>())).Returns(true);
        MockFileSystem.Mock.Setup(m => m.GetExtension(It.IsAny<string>())).Returns(".jpg");
        Mock<IFileStreamSource> file = new();
        file.Setup(m => m.OpenReadStream()).Returns(new MemoryStream());
        file.SetupGet(m => m.FileName).Returns(filename);
        */

        ImageFileService<MockFileSystem> service = new(_environment.Object);

        await service.AddImage(file.Object, product, true);

        MockFileSystem.Mock.Verify();
    }

    private (string? Filename, Product Product, Mock<IFileStreamSource> File) ArrangeAddImage(string? filename = $@"{ContentRoot}\Images\upload.jpg")
    {
        MockFileSystem.AddMock();
        Product product = new(1, "Orange Juice", 42.0, null, "Beverages", null, filename);
        string fullpath = string.Empty;
        MockFileSystem.Mock
            .Setup(m => m.GetFullPath(It.IsAny<string>()))
            .Callback((string path) => fullpath = path)
            .Returns(() => fullpath);
        MockFileSystem.Mock.Setup(m => m.FileDelete(It.IsAny<string>())).Verifiable(Times.Once);
        MockFileSystem.Mock.Setup(m => m.FileExists(It.IsAny<string>())).Returns(true);
        MockFileSystem.Mock.Setup(m => m.GetExtension(It.IsAny<string>())).Returns(".jpg");
        Mock<IFileStreamSource> file = new();
        file.Setup(m => m.OpenReadStream()).Returns(new MemoryStream());
        file.SetupGet(m => m.FileName).Returns(() => filename!);

        return (filename ?? product.ImageLocalPath, product, file);
    }
}
