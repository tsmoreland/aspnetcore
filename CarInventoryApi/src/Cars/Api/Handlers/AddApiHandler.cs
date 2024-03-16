using System.Net.Mime;
using CarInventory.Cars.Application.Features.Commands.Add;
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

internal static class AddApiHandler
{
    public static RouteGroupBuilder MapAddCar(this RouteGroupBuilder group, ref readonly CarsApiLinks links, params string[] authorizationPolicies)
    {
        OpenApiLink route = links[RouteName.AddCar];
        OpenApiLink get = links[RouteName.GetCarById];
        OpenApiLink update = links[RouteName.UpdateCar];
        OpenApiLink delete = links[RouteName.DeleteCarById];

        group
            .MapPost("/", Add)
            .RequireAuthorization(authorizationPolicies)
            .WithName(route.OperationId)
            .Produces<CarDetails>(StatusCodes.Status201Created, contentType: MediaTypeNames.Application.Json)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest, ExtendedMediaTypeNames.Application.JsonProblem)
            .WithOpenApi(Configure);

        return group;

        OpenApiOperation Configure(OpenApiOperation operation)
        {
            operation.OperationId = route.OperationId;
            operation.Description = route.Description;
            OpenApiResponse? response = operation.Responses
                .FirstOrDefault(kvp => kvp.Key == StatusCodes.Status201Created.ToString()).Value;
            if (response is null)
            {
                return operation;
            }
            response.Links = new Dictionary<string, OpenApiLink>
            {
                { get.OperationId, get },
                { update.OperationId, update },
                { delete.OperationId, delete },
            }.AsReadOnly();

            return operation;
        }
    }

    private static async Task<Results<CreatedAtRoute<CarDetails>, BadRequest<ProblemDetails>>> Add(HttpContext httpContext, [FromBody] AddCommand cmd, [FromServices] ICarRepository repository)
    {
        try
        {
            Car car = repository.Add(cmd.Make, cmd.Model, cmd.HorsePower, cmd.EngineType, cmd.FuelCapacityInLitres, cmd.NumberOfDoors, cmd.MpG);
            await repository.SaveChanges(default);
            // not sure of the route name here, check swagger
            return TypedResults.CreatedAtRoute(new CarDetails(car), nameof(RouteName.GetCarById), new { id = car.Id });
        }
        catch (ValidationException ex)
        {
            return TypedResults.BadRequest(ex.ToProblemDetails(statusCode: StatusCodes.Status400BadRequest));
        }
    }
}
