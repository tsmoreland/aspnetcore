using System.Collections.ObjectModel;
using System.Net.Mime;
using CarInventory.Cars.Application.Features.Commands.Remove;
using CarInventory.Cars.Application.Features.Queries.GetAll;
using CarInventory.Cars.Application.Features.Queries.GetById;
using CarInventory.Cars.Application.Features.Commands.Add;
using CarInventory.Cars.Application.Features.Commands.Update;
using CarInventory.Cars.Application.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace CarInventory.Cars.Application;

public static class OldMinimalApi
{
    /*
    private static readonly Lazy<Dictionary<RouteName, OpenApiLink>> s_linksByName = new(BuildLinksByName);
    private static Dictionary<RouteName, OpenApiLink> LinksByName => s_linksByName.Value;

    private static Dictionary<RouteName, OpenApiLink> BuildLinksByName()
    {
        OpenApiLink add = new() { OperationId = nameof(RouteName.AddCar) };
        OpenApiLink get = new() { OperationId = nameof(RouteName.GetCarById) };
        OpenApiLink getAll = new() { OperationId = nameof(RouteName.GetAllCars) };
        OpenApiLink update = new() { OperationId = nameof(RouteName.UpdateCar) };
        OpenApiLink delete = new() { OperationId = nameof(RouteName.DeleteCarById) };

        get.Parameters.Add("id", new RuntimeExpressionAnyWrapper { Any = new OpenApiString("$response.body#/id") });
        get.Description = "The `id` value returned in the response can be used as the `id` parameter in `GET /api/v1/cars/{id}`";

        update.Parameters.Add("id", new RuntimeExpressionAnyWrapper { Any = new OpenApiString("$response.body#/id") });
        //update.RequestBody = new RuntimeExpressionAnyWrapper { Any = new OpenApiSchema( ??? reference request body for PUT as a schema
        update.Description = "The `id` value returned in the response can be used as the `id` parameter in `PUT /api/v1/cars/{id}`";

        delete.Parameters.Add("id", new RuntimeExpressionAnyWrapper { Any = new OpenApiString("$response.body#/id") });
        delete.Description = "The `id` value returned in the response can be used as the `id` parameter in `DELETE /api/v1/cars/{id}`";


        return new Dictionary<RouteName, OpenApiLink>
        {
            { RouteName.AddCar, add },
            { RouteName.GetCarById, get },
            { RouteName.GetAllCars, getAll },
            { RouteName.UpdateCar, update },
            { RouteName.DeleteCarById, delete },
        };
    }

    public static RouteGroupBuilder MapCars(this RouteGroupBuilder group)
    {
        const string problemDetailsContentType = "application/problem+json";

        group
            .MapPost("/", async (AddCommand car, [FromServices] IMediator mediator) =>
            {
                CarDetails created = await mediator.Send(car);
                return Results.CreatedAtRoute("GetCarById", new { id = created.Id }, created);
            })
            .RequireAuthorization("application_admin")
            .WithName("AddCar")
            .Produces<CarDetails>(StatusCodes.Status201Created, contentType: MediaTypeNames.Application.Json)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest, problemDetailsContentType)
            .WithOpenApi(operation =>
            {
                operation.OperationId = "AddCar";
                OpenApiResponse? response = operation.Responses
                    .FirstOrDefault(kvp => kvp.Key == StatusCodes.Status201Created.ToString()).Value;
                if (response is null)
                {
                    return operation;
                }

                OpenApiLink get = LinksByName[RouteName.GetCarById];
                OpenApiLink update = LinksByName[RouteName.UpdateCar];
                OpenApiLink delete = LinksByName[RouteName.DeleteCarById];

                response.Links = new ReadOnlyDictionary<string, OpenApiLink>(new Dictionary<string, OpenApiLink>
                {
                    { get.OperationId, get },
                    { update.OperationId, update },
                    { delete.OperationId, delete },
                });

                return operation;
            });
        group
            .MapGet("/{id}", async Task<Results<Ok<CarDetails>, NotFound>> (Guid id, [FromServices] IMediator mediator) =>
            {
                CarDetails? car = await mediator.Send(new GetByIdQuery(id));
                return car is not null
                    ? TypedResults.Ok(car)
                    : TypedResults.NotFound();
            })
            .RequireAuthorization("application_client")
            .WithName("GetCarById")
            .Produces<CarDetails>(contentType: MediaTypeNames.Application.Json)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest, problemDetailsContentType)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound, problemDetailsContentType)
            .WithOpenApi(operation =>
            {
                operation.OperationId = "GetCarById";
                OpenApiParameter idParameter = operation.Parameters[0];
                idParameter.Description = "unique id of a car model";
                idParameter.Required = true;
                idParameter.In = ParameterLocation.Path;
                return operation;
            });
        group
            .MapGet("/", ([FromServices] IMediator mediator) => Results.Ok(mediator.CreateStream(new GetAllQuery())))
            .RequireAuthorization("application_client")
            .WithName("GetAllCars")
            .Produces<IAsyncEnumerable<CarDetails>>(contentType: MediaTypeNames.Application.Json)
            .WithOpenApi(operation =>
            {
                operation.OperationId = "GetAllCars";
                return operation;
            });
        group
            .MapPut("/{id}", async Task<Results<Ok<CarDetails>, NotFound>> ([FromServices] IMediator mediator, Guid id, [FromBody] UpdateCommand command) =>
            {
                CarDetails? car = await mediator.Send(command);
                return car is not null
                    ? TypedResults.Ok(car)
                    : TypedResults.NotFound();
            })
            .WithName("UpdateCar")
            .Produces<CarDetails>(contentType: MediaTypeNames.Application.Json)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest, problemDetailsContentType)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound, problemDetailsContentType)
            .WithOpenApi(operation =>
            {
                operation.OperationId = "UpdateCar";
                OpenApiParameter idParameter = operation.Parameters[0];
                idParameter.Description = "unique id of a car model";
                idParameter.Required = true;
                idParameter.In = ParameterLocation.Path;
                return operation;
            });
        group
            .MapDelete("/{id}", async (Guid id, [FromServices] IMediator mediator) =>
            {
                await mediator.Send(new RemoveCommand(id));
                return Results.NoContent();
            })
            .RequireAuthorization("application_admin")
            .WithName("RemoveCarById")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest, problemDetailsContentType)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound, problemDetailsContentType)
            .WithOpenApi(operation =>
            {
                operation.OperationId = "RemoveCarById";
                OpenApiParameter idParameter = operation.Parameters[0];
                idParameter.Description = "unique id of a car model";
                idParameter.Required = true;
                idParameter.In = ParameterLocation.Path;
                return operation;
            });

        return group;
    }
    */
}
