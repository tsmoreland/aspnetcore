using Microshop.Services.Rewards.RestApi.App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microshop.Services.Rewards.RestApi.App.Infrastructure.Data;

public sealed class RewardEntityTypeConfiguration : IEntityTypeConfiguration<Reward>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Reward> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.UserId);

        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.UserId).IsRequired().HasMaxLength(450);
        builder.Property(e => e.EarnedDate).IsRequired();
        builder.Property(e => e.RewardPoints).IsRequired();
        builder.Property(e => e.OrderId).IsRequired();
    }
}
