using MicroShop.Services.Products.ProductsApiApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroShop.Services.Products.ProductsApiApp.Infrastructure.Data;

public sealed class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => p.NormalizedName);
        builder.HasIndex(p => p.NormalizedCategoryName);

        builder.Property(p => p.Name)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_name")
            .HasMaxLength(200)
            .IsRequired()
            .IsUnicode();
        builder.Property(p => p.CategoryName)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_categoryName")
            .HasMaxLength(200)
            .IsRequired()
            .IsUnicode();
        builder.Property(p => p.NormalizedName)
            .HasMaxLength(200)
            .IsRequired()
            .IsUnicode();
        builder.Property(p => p.NormalizedCategoryName)
            .HasMaxLength(200)
            .IsRequired()
            .IsUnicode();
        builder.Property(p => p.Description)
            .HasMaxLength(500)
            .IsUnicode();
        builder.Property(p => p.Price)
            .IsRequired();
        builder.Property(p => p.ImageUrl)
            .HasMaxLength(200)
            .IsUnicode();
        builder.Property(p => p.ImageLocalPath)
            .HasMaxLength(260)
            .IsUnicode();
    }
}
