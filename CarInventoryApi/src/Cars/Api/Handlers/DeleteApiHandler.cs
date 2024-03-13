using System.Net.Mime;
using CarInventory.Cars.Application.Features.Shared;
using CarInventory.Cars.Domain.Contracts;
using CarInventory.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace CarInventory.Cars.Api.Handlers;

internal static class DeleteApiHandler
{

    public static RouteGroupBuilder MapDeleteCar(this RouteGroupBuilder group, ref readonly CarsApiLinks links, params string[] authorizationPolicies)
    {
        OpenApiLink route = links[RouteName.DeleteCarById];

        group
            .MapDelete("/{id}", Delete)
            .RequireAuthorization(authorizationPolicies)
            .WithName(route.OperationId)
            .Produces<CarDetails>(StatusCodes.Status204NoContent, contentType: MediaTypeNames.Application.Json)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound, ExtendedMediaTypeNames.Application.JsonProblem)
            .WithOpenApi(Configure);

        return group;

        OpenApiOperation Configure(OpenApiOperation operation)
        {
            operation.OperationId = route.OperationId;
            operation.Description = route.Description;
            OpenApiParameter idParameter = operation.Parameters[0];
            idParameter.Description = "unique id of a car model";
            idParameter.Required = true;
            idParameter.In = ParameterLocation.Path;
            return operation;
        }
    }

    private static async Task<Results<NoContent, NotFound>> Delete([FromRoute] Guid id, [FromServices] ICarRepository repository)
    {
        return await repository.DeleteCarById(id, default)
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
