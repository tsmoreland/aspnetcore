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
using TennisByTheSea.Application;
using TennisByTheSea.Domain.Configuration;
using TennisByTheSea.Infrastructure;
using TennisByTheSea.MvcApp.Models.Configuration;

namespace TennisByTheSea.MvcApp;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

        IServiceCollection services = builder.Services;
        ConfigurationManager configuration = builder.Configuration;

        services
            .AddControllersWithViews();
        services
            .ConfigureFeatureOptions(configuration)
            .Configure<WeatherApiOptions>(configuration.GetSection("ExternalServices::WeatherApi"))
            .AddMediatR(options => options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
            .AddApplicationServices(configuration)
            .AddInfrastructureServices(configuration);

        return builder;
    }
}
