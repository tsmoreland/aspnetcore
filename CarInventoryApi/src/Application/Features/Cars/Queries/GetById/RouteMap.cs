using System.Net.Mime;
using CarInventory.Application.Features.Cars.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace CarInventory.Application.Features.Cars.Queries.GetById;

internal static class RouteMap
{
    public static RouteGroupBuilder MapGetCarById(this RouteGroupBuilder group, ref readonly CarsApiLinks links)
    {
        _ = ref links;
        group
            .MapGet("/{id}", GetCarById)
            .RequireAuthorization("application_client")
            .WithName("GetCarById")
            .Produces<CarDetails>(contentType: MediaTypeNames.Application.Json)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest, problemDetailsContentType)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound, problemDetailsContentType)
            .WithOpenApi(Configure);
        return group;

        static OpenApiOperation Configure(OpenApiOperation operation)
        {
            operation.OperationId = "GetCarById";
            OpenApiParameter idParameter = operation.Parameters[0];
            idParameter.Description = "unique id of a car model";
            idParameter.Required = true;
            idParameter.In = ParameterLocation.Path;
            return operation;
        }
    }
}
