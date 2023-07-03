using GloboTicket.TicketManagement.Domain.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Contracts.Persistence.Specifications;
using GloboTicket.TicketManagement.Persistence.Configuration;
using GloboTicket.TicketManagement.Persistence.Infrastructure;
using GloboTicket.TicketManagement.Persistence.Repositories;
using GloboTicket.TicketManagement.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GloboTicket.TicketManagement.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddDbContext<GloboTicketDbContext>(optionsLifetime: ServiceLifetime.Singleton);
        services.AddDbContextFactory<GloboTicketDbContext>();

        services
            .AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>))
            .AddScoped<IEventRepository, EventRepository>()
            .AddScoped<ICategoryRepository, CategoryRepository>()
            .AddScoped<IOrderRepository, OrderRepository>();

        services
            .AddSingleton<IQueryableToEnumerableConverter, QueryableToEnumerableConverter>()
            .AddSingleton<IModelConfiguration<ModelBuilder, DbContextOptionsBuilder>, SqliteModelConfiguration>()
            .AddSingleton<IQueryBuilderFactory, QueryBuilderFactory>()
            .AddSingleton<IModelConfiguration<ModelBuilder, DbContextOptionsBuilder<GloboTicketDbContext>>, SqliteModelConfiguration>();

        return services;
    }
}
