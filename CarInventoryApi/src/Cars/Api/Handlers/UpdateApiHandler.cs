using System.Net.Mime;
using CarInventory.Cars.Application.Features.Commands.Update;
using CarInventory.Cars.Application.Features.Shared;
using CarInventory.Cars.Domain.Contracts;
using CarInventory.Cars.Domain.Models;
using CarInventory.Shared;
using CarInventory.Shared.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace CarInventory.Cars.Api.Handlers;

internal static class UpdateApiHandler
{
    public static RouteGroupBuilder MapUpdateCar(this RouteGroupBuilder group, ref readonly CarsApiLinks links, params string[] authorizationPolicies)
    {
        OpenApiLink route = links[RouteName.UpdateCar];
        string? description = links.GetDescriptionByNameOrDefault(RouteName.UpdateCar);

        OpenApiLink get = links[RouteName.GetCarById];
        OpenApiLink delete = links[RouteName.DeleteCarById];

        group
            .MapPut("/{id}", Update)
            .RequireAuthorization(authorizationPolicies)
            .WithName(route.OperationId)
            .Produces<CarDetails>(contentType: MediaTypeNames.Application.Json)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest, ExtendedMediaTypeNames.Application.JsonProblem)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound, ExtendedMediaTypeNames.Application.JsonProblem)
            .WithOpenApi(Configure);
        return group;

        OpenApiOperation Configure(OpenApiOperation operation)
        {
            operation.OperationId = route.OperationId;
            operation.Description = description;
            OpenApiParameter idParameter = operation.Parameters[0];
            idParameter.Description = "unique id of a car model";
            idParameter.Required = true;
            idParameter.In = ParameterLocation.Path;

            OpenApiResponse? response = operation.Responses
                .FirstOrDefault(kvp => kvp.Key == StatusCodes.Status200OK.ToString()).Value;
            if (response is null)
            {
                return operation;
            }
            response.Links = new Dictionary<string, OpenApiLink>
            {
                { get.OperationId, get },
                { delete.OperationId, delete },
            }.AsReadOnly();
            return operation;
        }
    }

    private static async Task<Results<Ok<CarDetails>, NotFound, BadRequest<ProblemDetails>>> Update([FromRoute] Guid id, [FromBody] UpdateCommand command, [FromServices] ICarRepository repository, CancellationToken cancellationToken)
    {
        Car? car = await repository.GetCarById(id, cancellationToken).ConfigureAwait(false);
        if (car is null)
        {
            return TypedResults.NotFound();
        }

        (int horsePower, decimal fuelCapacityInLitres, int numberOfDoors, decimal mpg) = command;

        try
        {
            car.HorsePower = horsePower;
            car.FuelCapacityInLitres = fuelCapacityInLitres;
            car.NumberOfDoors = numberOfDoors;
            car.MpG = mpg;
        }
        catch (ValidationException ex)
        {
            // TODO: gather all exceptions
            return TypedResults.BadRequest(ex.ToProblemDetails(statusCode: StatusCodes.Status400BadRequest));
        }

        return TypedResults.Ok(new CarDetails(car));
    }
}
