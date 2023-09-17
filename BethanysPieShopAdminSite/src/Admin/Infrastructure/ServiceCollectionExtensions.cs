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

using BethanysPieShop.Admin.Domain.Contracts;
using BethanysPieShop.Admin.Infrastructure.Persistence;
using BethanysPieShop.Admin.Infrastructure.Persistence.CachedRepositories;
using BethanysPieShop.Admin.Infrastructure.Persistence.Configuration;
using BethanysPieShop.Admin.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BethanysPieShop.Admin.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services
            .AddPooledDbContextFactory<AdminDbContext>(options => SqlServerConfiguration.ConfigureOptionsForPool(options, configuration, environment))
            .AddScoped(provider => provider.GetRequiredService<IDbContextFactory<AdminDbContext>>().CreateDbContext());

        services
            .AddScoped<CategoryRepository>()
            .AddScoped<OrderRepository>()
            .AddScoped<PieRepository>();

        services
            .AddScoped<ICategoryReadOnlyRepository>(p => p.GetRequiredService<CategoryRepository>())
            .Decorate<ICategoryReadOnlyRepository, CachedCategoryRepository>()
            .AddScoped<ICategoryRepository>(p => p.GetRequiredService<CategoryRepository>())
            .Decorate<ICategoryRepository, CachedCategoryRepository>()
            .AddScoped<IOrderReadOnlyRepository>(p => p.GetRequiredService<OrderRepository>())
            .AddScoped<IOrderRepository>(p => p.GetRequiredService<OrderRepository>())
            .AddScoped<IPieReadOnlyRepository>(p => p.GetRequiredService<PieRepository>())
            .Decorate<IPieReadOnlyRepository, CachedPieRepository>()
            .AddScoped<IPieRepository>(p => p.GetRequiredService<PieRepository>())
            .Decorate<IPieRepository, CachedPieRepository>();

        return services;
    }
}
