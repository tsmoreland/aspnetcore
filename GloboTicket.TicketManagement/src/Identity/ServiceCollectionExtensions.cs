using System.Text;
using System.Text.Json;
using GloboTicket.TicketManagement.Application.Models.Authentication;
using GloboTicket.TicketManagement.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using IAuthenticationService = GloboTicket.TicketManagement.Application.Contracts.Identity.IAuthenticationService;
using AuthenticationService = GloboTicket.TicketManagement.Identity.Services.AuthenticationService;

namespace GloboTicket.TicketManagement.Identity;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.AddDbContext<GloboTicketIdentityDbContext>(options =>
        {
            options
                .UseSqlite(configuration.GetConnectionString("GloboTicketIdentityConnection"),
                    b => b.MigrationsAssembly(typeof(GloboTicketIdentityDbContext).Assembly.FullName));

            if (!hostEnvironment.IsDevelopment())
            {
                options.UseModel(CompiledModels.GloboTicketIdentityDbContextModel.Instance);
            }
        });

        services
            .AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<GloboTicketIdentityDbContext>()
            .AddDefaultTokenProviders();

        services.AddTransient<IAuthenticationService, AuthenticationService>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"] ?? string.Empty))
                };

                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        return WriteProblemResponse(c.HttpContext, c.Response, 500, "internal error occurred.", c.Exception);
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        return WriteProblemResponse(context.HttpContext, context.Response, 401, "401 Not authorized");
                    },
                    OnForbidden = context =>
                        WriteProblemResponse(context.HttpContext, context.Response, 403, "403 Not authorized")
                };
            });


        return services;

        static Task WriteProblemResponse(HttpContext httpContext, HttpResponse response, int statusCode, string title, Exception? exception = null)
        {
            ProblemDetailsFactory problemDetailsFactory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
            ProblemDetails problem = problemDetailsFactory.CreateProblemDetails(httpContext, statusCode, title);

#if DEBUG
            problem.Detail = exception?.ToString();
#else
            _ = exception;
#endif

            string result = JsonSerializer.Serialize(problem);
            return response.WriteAsync(result);

        }
    }
}
