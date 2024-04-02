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
    }
}
