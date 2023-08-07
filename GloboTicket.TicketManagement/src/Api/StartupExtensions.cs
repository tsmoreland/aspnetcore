using System.Text.Json.Serialization;
using GloboTicket.TicketManagement.Api.Infrastructure.Filters;
using GloboTicket.TicketManagement.Api.Infrastructure.Swagger.Filters;
using GloboTicket.TicketManagement.Api.Middleware;
using GloboTicket.TicketManagement.Api.Models;
using GloboTicket.TicketManagement.Api.Services;
using GloboTicket.TicketManagement.Application;
using GloboTicket.TicketManagement.Domain.Contracts;
using GloboTicket.TicketManagement.Domain.Contracts.Identity;
using GloboTicket.TicketManagement.Identity;
using GloboTicket.TicketManagement.Infrastructure;
using GloboTicket.TicketManagement.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.OpenApi.Models;

namespace GloboTicket.TicketManagement.Api;

public static class StartupExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddApplicationServices()
            .AddInfrastructureServices(builder.Configuration)
            .AddPersistenceServices()
            .AddIdentityServices(builder.Configuration, builder.Environment);

        builder.Services.AddScoped<ILoggedInUserService, LoggedInUserService>();

        AddSwagger(builder.Services);

        builder.Services
            .AddProblemDetails()
            .AddControllers(static options =>
            {
                options.Filters.Add(new AddModelStateFeatureFilter());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
            });

        builder.Services
            .AddCors(static options =>
            {
                options.AddPolicy("Open", static builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });

        builder.Services
            .AddSingleton<IHostEnvironmentFacade, HostEnvironmentFacade>();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseExceptionHandler(); // custom should apply 90+% of the time, this is here as a fallback
        app.UseCustomExceptionHandler();
        app.UseStatusCodePages();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GloboTicket Ticket Management API"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors("Open");

        app.MapControllers();

        return app;
    }

    private static void AddSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "GloboTicket Ticket Management API"
            });
            c.EnableAnnotations();
            c.OperationFilter<FileResultContentTypeOperationFilter>();
        });
    }

    public static async Task ResetDatabaseAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        await ResetDatabaseAsync(scope.ServiceProvider.GetService<GloboTicketDbContext>()?.Database);
        await ResetDatabaseAsync(scope.ServiceProvider.GetService<GloboTicketIdentityDbContext>()?.Database, false);
    }

    private static async Task ResetDatabaseAsync(DatabaseFacade? database, bool deleteIExists = true)
    {
        if (database is null)
        {
            return;
        }

        try
        {
            if (deleteIExists)
            {
                await database.EnsureDeletedAsync();
            }
            await database.MigrateAsync();
        }
        catch (Exception)
        {
            // TODO: logging
        }
    }
}
