using CarInventory.Cars.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarInventory.Cars.Infrastructure.Persistence.Configuration;

public sealed class CarEntityTypeConfiguration : IEntityTypeConfiguration<Car>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable("cars");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => new { e.Make, e.Model }).IsUnique();

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Make).HasColumnName("make").HasMaxLength(50).IsUnicode().IsRequired();
        builder.Property(e => e.Model).HasColumnName("model").HasMaxLength(50).IsUnicode().IsRequired();
        builder.Property(e => e.HorsePower).HasColumnName("horse_power").IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field).HasField("_horsePower");
        builder.Property(e => e.Engine).HasColumnName("engine_type").IsRequired();
        builder.Property(e => e.FuelCapacityInLitres).HasColumnName("fuel_capacity").IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field).HasField("_fuelCapacityInLitres");
        builder.Property(e => e.NumberOfDoors).HasColumnName("number_of_doors").IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field).HasField("_numberOfDoors");
        builder.Property(e => e.MpG).HasColumnName("mpg").IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field).HasField("_mpg");
    }
}
