using System.Net.Mime;
using CarInventory.Application.Features.Cars.Commands.Add;
using CarInventory.Application.Features.Cars.Commands.Remove;
using CarInventory.Application.Features.Cars.Queries.GetAll;
using CarInventory.Application.Features.Cars.Queries.GetById;
using CarInventory.Application.Features.Cars.Shared;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace CarInventory.Api.RouteGroups;

public static class CarsGroup
{
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
                _ = operation; // TODO... set up links
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
                _ = operation;
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
}
