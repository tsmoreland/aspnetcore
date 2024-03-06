using CarInventory.Application.Features.Cars.Queries.GetAll;
using CarInventory.Application.Features.Cars.Queries.GetById;
using Microsoft.AspNetCore.Routing;

namespace CarInventory.Application;

public static class RouteGroupBuilderExtensions
{
    public static RouteGroupBuilder MapCarsApi(this RouteGroupBuilder group, string routePrefix, params string[] authorizationPolicies)
    {
        CarsApiLinks links = new(routePrefix);

        group
            .MapGetAllCars(in links, authorizationPolicies)
            .MapGetCarById(in links, authorizationPolicies);

        return group;
    }
}
