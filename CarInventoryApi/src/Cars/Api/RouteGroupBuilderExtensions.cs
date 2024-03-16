using CarInventory.Cars.Api.Handlers;
using Microsoft.AspNetCore.Routing;

namespace CarInventory.Cars.Api;

public static class RouteGroupBuilderExtensions
{
    public static RouteGroupBuilder MapCarsApi(this RouteGroupBuilder group, string routePrefix, params string[] authorizationPolicies)
    {
        CarsApiLinks links = new(routePrefix);

        group
            .MapAddCar(in links, authorizationPolicies)
            .MapGetSummaries(in links, authorizationPolicies)
            .MapGetCarById(in links, authorizationPolicies)
            .MapDeleteCar(in links, authorizationPolicies);

        return group;
    }
}
