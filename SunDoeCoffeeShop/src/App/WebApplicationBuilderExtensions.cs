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

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SunDoeCoffeeShop.App.Application.Contracts.Authentication;
using SunDoeCoffeeShop.App.Application.Features.Authentication.Commands.CertificateAuthenticationChallenged;
using SunDoeCoffeeShop.App.Application.Features.Authentication.Commands.CertificateValidated;
using SunDoeCoffeeShop.App.Application.Features.Authentication.Commands.CertificationAuthenticationFailed;
using SunDoeCoffeeShop.App.Data;
using SunDoeCoffeeShop.App.Services;

namespace SunDoeCoffeeShop.App;

/// <summary/>
internal static class WebApplicationBuilderExtensions
{
    /// <summary/>
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

        string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        services
            .AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString))
            .AddDatabaseDeveloperPageExceptionFilter();

        services
            .AddDefaultIdentity<IdentityUser>(static options =>
            {
                options.SignIn.RequireConfirmedAccount = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(5);
                options.Lockout.MaxFailedAccessAttempts = 100;
                options.Lockout.AllowedForNewUsers = false;

                // intentionally insecure.
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 1;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        AuthenticationBuilder authenticationBuilder = services
            .AddTransient<ICertificateAuthenticationFailedHandler, CertificateAuthenticationFailedHandler>()
            .AddTransient<ICertificateAuthenticationChallengedHandler, CertificateAuthenticationChallengedHandler>()
            .AddTransient<ICertificateValidatedHandler, CertificateValidatedHandler>()
            .AddAuthentication(static options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            });

        authenticationBuilder
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
            .AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services
            .ConfigureApplicationCookie(static options =>
            {
                options.Cookie.Name = "AuthCookie";
                //options.Cookie.HttpOnly = false; // intentionally insecure
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.IsEssential = true;
                options.Events = new CookieAuthenticationEvents
                {
                    OnSignedIn = context =>
                    {
                        _ = context;
                        return Task.CompletedTask;
                    },
                    OnSigningIn = context =>
                    {
                        _ = context;
                        return Task.CompletedTask;
                    },
                    OnValidatePrincipal = context =>
                    {
                        _ = context;
                        return Task.CompletedTask;
                    }
                };
                options.ExpireTimeSpan = TimeSpan.FromHours(2); // too long

                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";

                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });


        services.AddRazorPages();

        return builder;
    }
}
