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

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TennisByTheSea.Domain.Contracts;
using TennisByTheSea.Domain.Contracts.Services;
using TennisByTheSea.Infrastructure.Channels;
using TennisByTheSea.Infrastructure.ExternalApi;
using TennisByTheSea.Infrastructure.Persistence;
using TennisByTheSea.Infrastructure.Persistence.Configuration;
using TennisByTheSea.Shared.Exceptions;

namespace TennisByTheSea.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);

        string weatherUrl = configuration["ExternalServices:WeatherApi:Url"] ?? string.Empty;
        InvalidConfigurationException.ThrowIf(!Uri.TryCreate(weatherUrl, UriKind.Absolute, out Uri? baseWeatherUrl), "Invalid URL for weather API");

        string productsUrl = configuration["ExternalServices:ProductsApi:Url"] ?? string.Empty;
        InvalidConfigurationException.ThrowIf(!Uri.TryCreate(productsUrl, UriKind.Absolute, out Uri? baseProductsUrl), "Invalid URL for Products API");

        services
            .AddTransient<IWeatherApiClient, WeatherApiClient>()
            .AddHttpClient<WeatherApiClient>(client => client.BaseAddress = baseWeatherUrl);
        services
            .AddHttpClient("Products", client => client.BaseAddress = baseProductsUrl);

        services
            .AddSingleton<IFileProcessingChannel, FileProcessingChannel>()
            .AddSingleton<IReadOnlyFileProcessingChannel>(provider => provider.GetRequiredService<IFileProcessingChannel>())
            .AddMediatR(options => options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services
            .AddDbContext<TennisByTheSeaDbContext>(optionsLifetime: ServiceLifetime.Singleton)
            .AddDbContextFactory<TennisByTheSeaDbContext>()
            .AddSingleton<IModelConfiguration, SqliteModelConfiguration>();

        services
            .AddTransient<ITimeService, TimeService>();

        return services;
    }
}
