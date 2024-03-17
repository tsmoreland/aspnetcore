using CarInventory.Cars.Api.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CarInventory.Cars.Api;

public static class RouteGroupBuilderExtensions
{
    public static RouteGroupBuilder MapCarsApi(this RouteGroupBuilder group, string routePrefix,
        string[]? addPolicies = null, string[]? getPolicies = null, string[]? updatePolicies = null, string[]? deletePolicies = null)
    {
        CarsApiLinks links = new(routePrefix);

        group
            .MapAddCar(in links, addPolicies ?? [])
            .MapGetSummaries(in links, getPolicies ?? [])
            .MapGetCarById(in links, getPolicies ?? [])
            .MapUpdateCar(in links, updatePolicies ?? [])
            .MapDeleteCar(in links, deletePolicies ?? [])
            .WithTags("Car Inventory CRUD");

        return group;
    }
}
