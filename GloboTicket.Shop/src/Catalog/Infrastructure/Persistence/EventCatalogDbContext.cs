// 
// Copyright © 2023 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using GloboTicket.Shop.Catalog.Domain.Models;
using GloboTicket.Shop.Shared.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GloboTicket.Shop.Catalog.Infrastructure.Persistence;

public sealed class EventCatalogDbContext : DbContext
{
    private readonly IModelConfiguration<ModelBuilder, DbContextOptionsBuilder> _modelConfiguration;

    /// <inheritdoc />
    public EventCatalogDbContext(
        DbContextOptions<EventCatalogDbContext> options,
        IModelConfiguration<ModelBuilder, DbContextOptionsBuilder> modelConfiguration)
        : base(options)
    {
        _modelConfiguration = modelConfiguration ?? throw new ArgumentNullException(nameof(modelConfiguration));
    }

    public DbSet<Concert> Concerts => Set<Concert>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _modelConfiguration.ConfigureModel(modelBuilder);
        SeedData(modelBuilder);
    }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _modelConfiguration.ConfigureContext(optionsBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        Guid userId = Guid.NewGuid(); // this shouild be provided by some service at a later point
        string? user = userId != Guid.Empty
            ? userId.ToString()
            : null;

        foreach (EntityEntry<IAuditableEntity> entry in ChangeTracker.Entries<IAuditableEntity>().Where(e => e.State is EntityState.Added or EntityState.Modified))
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (user is not null)
                    {
                        entry.Entity.SetAuditCreatedDetails(user);
                    }
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdateConcurrencyToken();
                    if (user is not null)
                    {
                        entry.Entity.SetAuditUpdatedDetails(user);
                    }
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);

    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // see: https://github.com/vriesmarcel/asp-dot-net-core-6-kubernetes/blob/eb435d4cac8ad66725b32ef1d1da95531e43e726/catalog/DBContext/EventCatalogDBContext.cs#L20
        EntityTypeBuilder<Concert> concertEntity = modelBuilder.Entity<Concert>();

        concertEntity.HasData(new Concert(
            Guid.Parse("{EE272F8B-6096-4CB6-8625-BB4BB2D89E8B}"),
            "John Egbert Live",
            65,
            "John Egbert",
            DateTime.Now.AddMonths(6),
            "Join John for his farwell tour across 15 continents. John really needs no introduction since he has already mesmerized the world with his banjo.",
            "/img/banjo.jpg",
            null,
            Guid.NewGuid().ToString("N")));

        concertEntity.HasData(new Concert(
            Guid.Parse("{3448D5A4-0F72-4DD7-BF15-C14A46B26C00}"),
            "The State of Affairs: Michael Live!",
            85,
            "Michael Johnson",
            DateTime.Now.AddMonths(9),
            "Michael Johnson doesn't need an introduction. His 25 concert across the globe last year were seen by thousands. Can we add you to the list?",
            "/img/michael.jpg",
            null,
            Guid.NewGuid().ToString("N")));

        concertEntity.HasData(new Concert(
            Guid.Parse("{B419A7CA-3321-4F38-BE8E-4D7B6A529319}"),
            "Clash of the DJs",
            85,
            "DJ 'The Mike'",
            DateTime.Now.AddMonths(4),
            "DJs from all over the world will compete in this epic battle for eternal fame.",
            "/img/dj.jpg",
            null,
            Guid.NewGuid().ToString("N")));

        concertEntity.HasData(new Concert(
            Guid.Parse("{62787623-4C52-43FE-B0C9-B7044FB5929B}"),
            "Spanish guitar hits with Manuel",
            25,
            "Manuel Santinonisi",
            DateTime.Now.AddMonths(4),
            "Get on the hype of Spanish Guitar concerts with Manuel.",
            "/img/guitar.jpg",
            null,
            Guid.NewGuid().ToString("N")));

        concertEntity.HasData(new Concert(
            Guid.Parse("{ADC42C09-08C1-4D2C-9F96-2D15BB1AF299}"),
            "To the Moon and Back",
            135,
            "Nick Sailor",
            DateTime.Now.AddMonths(8),
            "The critics are over the moon and so will you after you've watched this sing and dance extravaganza written by Nick Sailor, the man from 'My dad and sister'.",
            "/img/musical.jpg",
            null,
            Guid.NewGuid().ToString("N")));
    }
}
