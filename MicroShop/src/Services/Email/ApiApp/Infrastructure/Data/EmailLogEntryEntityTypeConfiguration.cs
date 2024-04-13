using MicroShop.Services.Email.ApiApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroShop.Services.Email.ApiApp.Infrastructure.Data;

public sealed class EmailLogEntryEntityTypeConfiguration : IEntityTypeConfiguration<EmailLogEntry>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<EmailLogEntry> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.NormalizedEmailAddress);

        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(e => e.EmailAddress).IsRequired().IsUnicode().HasMaxLength(200);
        builder.Property(e => e.NormalizedEmailAddress).IsRequired().IsUnicode().HasMaxLength(200);
        builder.Property(e => e.Message).IsRequired().IsUnicode().HasMaxLength(1000);
        builder.Property(e => e.EmailSent);
    }


}
