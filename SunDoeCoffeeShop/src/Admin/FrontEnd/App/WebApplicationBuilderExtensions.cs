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

using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Serilog;
using SunDoeCoffeeShop.Admin.FrontEnd.App.Application.Contracts.Authentication;
using SunDoeCoffeeShop.Admin.FrontEnd.App.Application.Features.Authentication.Commands.CertificateAuthenticationChallenged;
using SunDoeCoffeeShop.Admin.FrontEnd.App.Application.Features.Authentication.Commands.CertificateValidated;
using SunDoeCoffeeShop.Admin.FrontEnd.App.Application.Features.Authentication.Commands.CertificationAuthenticationFailed;
using SunDoeCoffeeShop.Admin.FrontEnd.App.Services;
using SunDoeCoffeeShop.Shared.AuthPersistence;
using SunDoeCoffeeShop.Shared.Roles;

namespace SunDoeCoffeeShop.Admin.FrontEnd.App;

internal static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.WebHost
            .ConfigureKestrel(static options =>
            {
                options.AddServerHeader = true; // normally this would be false
                options.ConfigureHttpsDefaults(static options =>
                {
                    options.CheckCertificateRevocation = false;
                    options.ClientCertificateValidation = (_, _, _) => true; // really not secure
                    options.CheckCertificateRevocation = false; // also a bad idea
                    options.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
                });
            });

        builder.Host
            .UseSerilog(static (context, loggerConfiguration) => loggerConfiguration
                .WriteTo.Console()
                .ReadFrom.Configuration(context.Configuration));

        IServiceCollection services = builder.Services;
        IConfiguration configuration = builder.Configuration;
        IHostEnvironment env = builder.Environment;

        services
            .AddAuthPersistence(configuration, env)
            .AddDatabaseDeveloperPageExceptionFilter();

        services
            .AddAuthorization(static options =>
            {
                options
                    .AddPolicy("administrator", static policy =>
                        policy
                            .RequireRole(RoleName.Administrator)
                            .RequireAuthenticatedUser());

            });

        services
            .AddTransient<ICertificateAuthenticationFailedHandler, CertificateAuthenticationFailedHandler>()
            .AddTransient<ICertificateAuthenticationChallengedHandler, CertificateAuthenticationChallengedHandler>()
            .AddTransient<ICertificateValidatedHandler, CertificateValidatedHandler>()
            .AddAuthentication(static options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                options.DefaultAuthenticateScheme = CertificateAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CertificateAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCertificate(options =>
            {
                options.AllowedCertificateTypes = CertificateTypes.All;
                options.Events = new CertificateAuthenticationEvents
                {
                    OnAuthenticationFailed = CertificateAuthenticationEventHandler.OnAuthenticationFailed,
                    OnCertificateValidated = CertificateAuthenticationEventHandler.OnCertificateValidated,
                    OnChallenge = CertificateAuthenticationEventHandler.OnChallenge,
                };
            });

        services
            .AddMvcCore(static options =>
            {
                AuthorizationPolicy policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireRole(RoleName.Administrator)
                        .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddRazorPages();

        return builder;
    }
}
