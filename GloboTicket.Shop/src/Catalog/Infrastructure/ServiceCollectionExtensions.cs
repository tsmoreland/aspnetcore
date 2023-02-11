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

using GloboTicket.Shop.Catalog.Application.Contracts.Persistence;
using GloboTicket.Shop.Catalog.Infrastructure.Persistence;
using GloboTicket.Shop.Catalog.Infrastructure.Persistence.Configuration;
using GloboTicket.Shop.Catalog.Infrastructure.Persistence.Repositories;
using GloboTicket.Shop.Catalog.Infrastructure.Persistence.Specifications;
using GloboTicket.Shop.Shared.Contracts.Persistence;
using GloboTicket.Shop.Shared.Contracts.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GloboTicket.Shop.Catalog.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCatalogInfrastructure(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddDbContext<EventCatalogDbContext>(optionsLifetime: ServiceLifetime.Singleton);
        services.AddDbContextFactory<EventCatalogDbContext>();

        services
            .AddScoped<IConcertRepository, ConcertRepository>()
            .AddScoped<IReadOnlyConcertRepository, ConcertRepository>();

        services
            .AddSingleton<IQueryableToEnumerableConverter, QueryableToEnumerableConverter>()
            .AddSingleton<IModelConfiguration<ModelBuilder, DbContextOptionsBuilder>, SqlServerModelConfiguration>()
            .AddSingleton<IQueryBuilderFactory, QueryBuilderFactory>();

        return services;
    }

}
