using GloboTicket.TicketManagement.Domain.Common;
using GloboTicket.TicketManagement.Domain.Contracts.Identity;
using GloboTicket.TicketManagement.Domain.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GloboTicket.TicketManagement.Persistence;

public sealed class GloboTicketDbContext : DbContext
{
    private readonly IModelConfiguration<ModelBuilder, DbContextOptionsBuilder> _modelConfiguration;
    private readonly ILoggedInUserService _loggedInUserService;

    /// <inheritdoc />
    public GloboTicketDbContext(DbContextOptions<GloboTicketDbContext> options,
        IModelConfiguration<ModelBuilder, DbContextOptionsBuilder> modelConfiguration,
        ILoggedInUserService loggedInUserService)
        : base(options)
    {
        _modelConfiguration = modelConfiguration ?? throw new ArgumentNullException(nameof(modelConfiguration));
        _loggedInUserService = loggedInUserService ?? throw new ArgumentNullException(nameof(loggedInUserService));
    }

    public DbSet<Event> Events => Set<Event>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Order> Orders => Set<Order>();

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

    private static void SeedData(ModelBuilder modelBuilder)
    {
        Guid concertId = new("2AB3D254-C85A-4232-A061-D0C3F5516D2E");
        Guid musicalId = new("5B94F780-CF1F-484F-9403-4A3531B26C1F");
        Guid playId = new("96C73B1D-CC44-4AC4-B034-EF503BA298BA");
        Guid conferenceId = new("50CE56E9-8C15-4CE1-8F30-501B25394E55");

        EntityTypeBuilder<Category> categoryEntity = modelBuilder.Entity<Category>();
        categoryEntity.HasData(new Category { CategoryId = concertId, Name = "Concerts" });
        categoryEntity.HasData(new Category { CategoryId = musicalId, Name = "Musicals" });
        categoryEntity.HasData(new Category { CategoryId = playId, Name = "Plays" });
        categoryEntity.HasData(new Category { CategoryId = conferenceId, Name = "Conferences" });

        EntityTypeBuilder<Event> eventEntity = modelBuilder.Entity<Event>();
        eventEntity.HasData(new Event
        {
            EventId = new Guid("2F620341-FA98-48B1-8AF4-5418B1E109E1"),
            Name = "John Egbert Live",
            Price = 65,
            Artist = "John Egbert",
            Date = DateTime.Now.AddMonths(6),
            Description = "Join John for his farewell tour across 15 continents",
            ImageUrl = "https://example.com/images/example.jpg",
            CategoryId = concertId,
        });
        eventEntity.HasData(new Event
        {
            EventId = new Guid("B5775CCA-5533-4BE5-88F8-F0EDC7BE6E04"),
            Name = "The State of Affairs: Michael Live!",
            Price = 85,
            Artist = "Michael Johnson",
            Date = DateTime.Now.AddMonths(9),
            Description = "Michael Johnson doesn't need an introduction",
            ImageUrl = "https://example.com/images/example.jpg",
            CategoryId = concertId,
        });
        eventEntity.HasData(new Event
        {
            EventId = new Guid("9F6BA16E-A130-4AE1-9F0C-07F695636B4B"),
            Name = "Clash of the DJs",
            Price = 85,
            Artist = "DJ 'The Mike'",
            Date = DateTime.Now.AddMonths(4),
            Description = "DJs from all over the world will complete in this empic battle for eternal fame.",
            ImageUrl = "https://example.com/images/example.jpg",
            CategoryId = concertId,
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        Guid userId = _loggedInUserService.UserId;
        string? user = userId != Guid.Empty
            ? userId.ToString()
            : null;

        foreach (EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.Now;
                    entry.Entity.CreatedBy = user;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedDate = DateTime.Now;
                    entry.Entity.LastModifiedBy = user;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
