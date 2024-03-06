using System.Net.Mime;
using CarInventory.Application.Features.Cars.Shared;
using CarInventory.Domain.Contracts;
using CarInventory.Domain.Models;
using CarInventory.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace CarInventory.Application.Features.Cars.Queries.GetById;

internal static class GetByIdApiHandler
{
    public static RouteGroupBuilder MapGetCarById(this RouteGroupBuilder group, ref readonly CarsApiLinks links, params string[] authorizationPolicies)
    {
        _ = ref links;
        group
            .MapGet("/{id}", GetById)
            .RequireAuthorization(authorizationPolicies)
            .WithName("GetCarById")
            .Produces<CarDetails>(contentType: MediaTypeNames.Application.Json)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest, ExtendedMediaTypeNames.Application.JsonProblem)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound, ExtendedMediaTypeNames.Application.JsonProblem)
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

    private static async Task<Results<Ok<CarDetails>, NotFound>> GetById(Guid id, [FromServices] ICarRepository repository, CancellationToken cancellationToken)
    {
        Car? car = await repository.GetCarById(id, cancellationToken);
        return car is not null
            ? TypedResults.Ok(new CarDetails(car))
            : TypedResults.NotFound();
    }
}
