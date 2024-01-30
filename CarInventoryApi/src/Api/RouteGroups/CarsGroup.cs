using CarInventory.Application.Features.Cars.Commands.Add;
using CarInventory.Application.Features.Cars.Commands.Remove;
using CarInventory.Application.Features.Cars.Queries.GetAll;
using CarInventory.Application.Features.Cars.Queries.GetById;
using CarInventory.Application.Features.Cars.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarInventory.Api.RouteGroups;

public static class CarsGroup
{
    public static RouteGroupBuilder MapCars(this RouteGroupBuilder group)
    {
        group
            .MapPost("/", async (AddCommand car, [FromServices] IMediator mediator) => await mediator.Send(car))
            .RequireAuthorization("application_admin")
            .WithName("AddCar")
            .WithOpenApi();
        group
            .MapGet("/{id}", async (Guid id, [FromServices] IMediator mediator) =>
            {
                CarDetails? car = await mediator.Send(new GetByIdQuery(id));
                return car is not null
                    ? Results.Ok(car)
                    : Results.NotFound();
            })
            .RequireAuthorization("application_client")
            .WithName("GetCarById")
            .WithOpenApi();
        group
            .MapGet("/", ([FromServices] IMediator mediator) => Results.Ok(mediator.CreateStream(new GetAllQuery())))
            .RequireAuthorization("application_client")
            .WithName("GetAllCars")
            .WithOpenApi();
        group
            .MapDelete("/{id}", async (Guid id, [FromServices] IMediator mediator) =>
            {
                await mediator.Send(new RemoveCommand(id));
                return Results.NoContent();
            })
            .RequireAuthorization("application_admin")
            .WithName("RemoveCarById")
            .WithOpenApi();

        return group;
    }
}
