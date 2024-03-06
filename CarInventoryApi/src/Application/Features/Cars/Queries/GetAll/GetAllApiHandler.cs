using System.Net.Mime;
using CarInventory.Application.Features.Cars.Shared;
using CarInventory.Domain.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace CarInventory.Application.Features.Cars.Queries.GetAll;

internal static class GetAllApiHandler
{
    public static RouteGroupBuilder MapGetSummaries(this RouteGroupBuilder group, ref readonly CarsApiLinks links, params string[] authorizationPolicies)
    {
        OpenApiLink route = links[RouteName.GetAllCars];
        // TODO: change return type, this should return a summary response not the full details

        group
            .MapGet("/", static ([FromServices] ICarRepository repository) => Results.Ok(repository.GetCarSummaries()))
            .RequireAuthorization(authorizationPolicies)
            .WithName(nameof(RouteName.GetAllCars))
            .Produces<IAsyncEnumerable<CarDetails>>(contentType: MediaTypeNames.Application.Json)
            .WithOpenApi(operation =>
            {
                operation.OperationId = route.OperationId;
                operation.Description = route.Description;
                return operation;
            });

        return group;
    }
}
