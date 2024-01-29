using CarInventory.Domain.Contracts;
using CarInventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CarInventory.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder  healthChecksBuilder, IHostEnvironment environment)
    {
        string? connectionString = configuration.GetConnectionString("Default");
        if (connectionString is not { Length: > 0 })
        {
            throw new KeyNotFoundException("Default connection not found");
        }

        services
            .AddPooledDbContextFactory<CarsDbContext>(options =>
            {
                options.UseSqlite(connectionString, o => o.MigrationsAssembly(typeof(CarsDbContext).Assembly.FullName)); 
                if (!environment.IsDevelopment())
                {
                    options.UseModel(Persistence.CompiledModels.CarsDbContextModel.Instance);
                }
            }, poolSize: 64)
            .AddScoped<CarsScopedDbContextFactory>()
            .AddScoped(sp => sp.GetRequiredService<CarsScopedDbContextFactory>().CreateDbContext());

        services.AddScoped<ICarRepository, CarRepository>();

        healthChecksBuilder.AddCheck<DatabaseHealthCheck>("Database");

        return services;
    }
}
