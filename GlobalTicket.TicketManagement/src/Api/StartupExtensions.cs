using System.Text.Json.Serialization;
using GlobalTicket.TicketManagement.Api.Infrastructure.Filters;
using GlobalTicket.TicketManagement.Api.Infrastructure.Swagger.Filters;
using GlobalTicket.TicketManagement.Api.Middleware;
using GlobalTicket.TicketManagement.Application;
using GlobalTicket.TicketManagement.Infrastructure;
using GlobalTicket.TicketManagement.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace GlobalTicket.TicketManagement.Api;

public static class StartupExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddApplicationServices()
            .AddInfrastructureServices(builder.Configuration)
            .AddPersistenceServices();

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
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GlobalTicket Ticket Management API"));
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
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "GlobalTicket Ticket Management API"
            });
            c.OperationFilter<FileResultContentTypeOperationFilter>();
        });
    }

    public static async Task RestDatabaseAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        try
        {
            GlobalTicketDbContext? dbContext = scope.ServiceProvider.GetService<GlobalTicketDbContext>();
            if (dbContext is null)
            {
                // Log failure
                return;
            }

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.MigrateAsync();
        }
        catch (Exception)
        {
            // TODO: logging
        }
    }
}
