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

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Serilog;
using SunDoeCoffeeShop.Shared.AuthPersistence;

namespace SunDoeCoffeeShop.FrontEnd.App;

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
            .AddEntityFrameworkStores<AuthDbContext>();

        services
            .AddAuthentication(static options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
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
