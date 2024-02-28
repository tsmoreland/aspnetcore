using CarInventory.Application.Features.Cars.Queries.GetById;
using Microsoft.AspNetCore.Routing;

namespace CarInventory.Application;

public static class RouteGroup
{
    public static RouteGroupBuilder MapCarsApi(this RouteGroupBuilder group, string routePrefix)
    {
        CarsApiLinks links = new(routePrefix);

        group
            .MapGetCarById(in links);

        return group;
    }
}
