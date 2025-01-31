﻿using System.Text;
using System.Text.Json;
using CarInventory.App.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CarInventory.App.Configuration;

public static class HealthChecksConfiguration
{
    public static HealthCheckOptions GetOptions(Func<HealthCheckRegistration, bool>? predicate = null)
    {
        return new HealthCheckOptions
        {
            Predicate = predicate,
            AllowCachingResponses = false,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
            },
            ResponseWriter = WriteHealthResponse,
        };
    }

    private static async Task WriteHealthResponse(HttpContext context, HealthReport report)
    {
        ILoggerFactory? loggerFactory = context.RequestServices.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
        ILogger? logger = loggerFactory?.CreateLogger(nameof(HealthChecksConfiguration));
        switch (report.Status)
        {
            case HealthStatus.Healthy:
                logger?.LogInformation("{HealthReport}", report.Status);
                break;
            case HealthStatus.Degraded:
                logger?.LogWarning("{HealthReport} {HealthEntries}", report.Status, report.Entries.Values.Select(e => e.Description));
                break;
            case HealthStatus.Unhealthy:
                logger?.LogError("{HealthReport} {HealthEntries}", report.Status, report.Entries.Values.Select(e => e.Description));
                break;
        }

        var options = new JsonWriterOptions { Indented = false };

        bool authenticated = context.User.Identity?.IsAuthenticated == true;
        using MemoryStream memoryStream = new();

        Utf8JsonWriter jsonWriter = new(memoryStream, options);
        await using (jsonWriter.ConfigureAwait(false))
        {
            jsonWriter.WriteStartObject();
            jsonWriter.WriteString("status", report.Status.ToString());
            if (authenticated)
            {
                jsonWriter.WriteStartObject("results");

                foreach (var healthReportEntry in report.Entries)
                {
                    jsonWriter.WriteStartObject(healthReportEntry.Key.ToSnakeCase());
                    jsonWriter.WriteString("status",
                        healthReportEntry.Value.Status.ToString());
                    if (healthReportEntry.Value.Description is { Length: > 0 })
                    {
                        jsonWriter.WriteString("description",
                            healthReportEntry.Value.Description);
                    }

                    if (healthReportEntry.Value.Data.Any())
                    {
                        jsonWriter.WriteStartObject("data");
                        foreach (var item in healthReportEntry.Value.Data)
                        {
                            jsonWriter.WritePropertyName(item.Key.ToSnakeCase());

                            JsonSerializer.Serialize(jsonWriter, item.Value,
                                item.Value?.GetType() ?? typeof(object));
                        }

                        jsonWriter.WriteEndObject();
                    }

                    jsonWriter.WriteEndObject();
                }
                jsonWriter.WriteEndObject();
            }

            jsonWriter.WriteEndObject();
        }

        var response = Encoding.UTF8.GetString(memoryStream.ToArray());
        await context.Response.WriteAsync(response).ConfigureAwait(false);
    }
}
