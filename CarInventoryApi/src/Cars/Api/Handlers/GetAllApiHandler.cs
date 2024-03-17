using System.Net.Mime;
using CarInventory.Cars.Domain.Contracts;
using CarInventory.Cars.Domain.Models.Projections;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace CarInventory.Cars.Api.Handlers;

internal static class GetAllApiHandler
{
    public static RouteGroupBuilder MapGetSummaries(this RouteGroupBuilder group, ref readonly CarsApiLinks links, params string[] authorizationPolicies)
    {
        OpenApiLink route = links[RouteName.GetAllCars];
        string? description = links.GetDescriptionByNameOrDefault(RouteName.GetAllCars);
        // TODO: change return type, this should return a summary response not the full details

        group
            .MapGet("/", static ([FromServices] ICarRepository repository) => Results.Ok(repository.GetCarSummaries()))
            .RequireAuthorization(authorizationPolicies)
            .WithName(nameof(RouteName.GetAllCars))
            .Produces<IAsyncEnumerable<CarSummary>>(contentType: MediaTypeNames.Application.Json)
            .WithOpenApi(operation =>
            {
                operation.OperationId = route.OperationId;
                operation.Description = description;
                return operation;
            });

        return group;
    }
}
