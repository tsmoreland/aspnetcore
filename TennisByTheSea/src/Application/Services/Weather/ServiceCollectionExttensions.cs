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

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TennisByTheSea.Application.BackgroundServices.WeatherCache;
using TennisByTheSea.Domain.Contracts.Services.Weather;

namespace TennisByTheSea.Application.Services.Weather;

public static class ServiceCollectionExttensions
{
    public static IServiceCollection AddWeatherForecasting(this IServiceCollection services,
        IConfiguration config)
    {
        if (config.GetValue<bool>("Features:WeatherForecasting:Enable"))
        {
            services.TryAddSingleton<IWeatherForecaster, WeatherForecaster>();
            services.Decorate<IWeatherForecaster, CachedWeatherForecaster>();
            services.AddHostedService<WeatherCacheService>();
        }
        else
        {
            services.TryAddSingleton<IWeatherForecaster, DisabledWeatherForecaster>();
        }

        return services;
    }
}
