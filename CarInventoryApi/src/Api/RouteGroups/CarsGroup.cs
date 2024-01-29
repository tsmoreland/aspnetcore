using CarInventory.Application.Features.Cars.Commands.Add;
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
            .WithName("GetCarById")
            .WithOpenApi();

        return group;
    }
}
