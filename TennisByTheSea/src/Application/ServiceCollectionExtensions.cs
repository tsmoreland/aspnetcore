﻿//
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
using TennisByTheSea.Application.BackgroundServices.FileProcessing;
using TennisByTheSea.Application.Configuration;
using TennisByTheSea.Application.Services.Unavailability;
using TennisByTheSea.Application.Services.Weather;
using TennisByTheSea.Domain.Configuration;
using TennisByTheSea.Domain.Models;

namespace TennisByTheSea.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);

        services
            .AddWeatherForecasting(configuration)
            .AddHostedService<FileProcessingService>()
            .AddMediatR(options => options.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), typeof(Court).Assembly))
            .AddUnavailabilityProviders();

        services
            .Configure<BookingOptions>(configuration.GetSection("CourtBookings"))
            .Configure<ClubOptions>(configuration.GetSection("ClubSettings"))
            .Configure<ContentOptions>(configuration.GetSection("Content"))
            .Configure<GreetingOptions>(configuration.GetSection("Greeting"))
            .Configure<HomePageOptions>(configuration.GetSection("HomePage"))
            .Configure<MembershipOptions>(configuration.GetSection("Membership"))
            .Configure<ScoreProcesingOptions>(configuration.GetSection("ScoreProcessing"))
            .Configure<WeatherForecastingOptions>(configuration.GetSection("WeatherForecasting"));

        return services;
    }
}
