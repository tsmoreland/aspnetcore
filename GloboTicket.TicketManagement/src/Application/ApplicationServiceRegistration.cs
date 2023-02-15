using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GloboTicket.TicketManagement.Application;

public static class ApplicationServiceRegistration
{
    /// <summary>
    /// Register application services with Inversion of Control container <paramref name="services"/>
    /// </summary>
    /// <param name="services">IoC container to register services against</param>
    /// <returns><paramref name="services"/> to allow call chaining</returns>
    /// <exception cref="ArgumentNullException">
    /// if <paramref name="services"/> is <see langword="null"/>
    /// </exception>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(options => options.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        return services;
    }
}
