//
// Copyright © 2022 Terry Moreland
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

using Microsoft.Extensions.DependencyInjection;
using WiredBrainCoffee.EmployeeManager.Domain.Contracts;
using WiredBrainCoffee.EmployeeManager.Infrastructure.Configuration;
using WiredBrainCoffee.EmployeeManager.Infrastructure.Contracts;
using WiredBrainCoffee.EmployeeManager.Infrastructure.Repositories;

namespace WiredBrainCoffee.EmployeeManager.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);


        services
            .AddDbContext<EmployeeManagerDbContext>(optionsLifetime: ServiceLifetime.Singleton)
            .AddDbContextFactory<EmployeeManagerDbContext>();

        services
            .AddScoped<IEmployeeRepository, EmployeeRepository>()
            .AddScoped<IDepartmentRepository, DepartmentRepository>();

        services
            .AddSingleton<IModelConfiguration, SqliteModelConfiguration>()
            .AddSingleton<IRepositoryFactory, RepositoryFactory>();

        return services;
    }
}
