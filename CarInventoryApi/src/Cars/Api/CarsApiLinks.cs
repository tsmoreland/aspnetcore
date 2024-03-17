using System.Diagnostics.CodeAnalysis;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace CarInventory.Cars.Api;

internal readonly struct CarsApiLinks
{
    private readonly string _routePrefix = string.Empty;
    private readonly Lazy<IReadOnlyDictionary<RouteName, OpenApiLink>> _linksByName;
    private readonly Lazy<IReadOnlyDictionary<RouteName, string>> _descriptionsByName;

    public CarsApiLinks(string routePrefix)
    {
        _routePrefix = routePrefix;
        _linksByName = new Lazy<IReadOnlyDictionary<RouteName, OpenApiLink>>(BuildLinksByName);
        _descriptionsByName = new Lazy<IReadOnlyDictionary<RouteName, string>>(BuildDescriptionsByName);
    }


    public OpenApiLink this[RouteName routeName] => _linksByName.Value[routeName];
    public bool ContainsKey(RouteName routeName) => _linksByName.Value.ContainsKey(routeName);
    public bool TryGetValue(RouteName routeName, [NotNullWhen(true)] out OpenApiLink? link) => _linksByName.Value.TryGetValue(routeName, out link);

    public string? GetDescriptionByNameOrDefault(RouteName routeName) => _descriptionsByName.Value.GetValueOrDefault(routeName);

    private Dictionary<RouteName, OpenApiLink> BuildLinksByName()
    {
        OpenApiLink add = new() { OperationId = nameof(RouteName.AddCar) };
        OpenApiLink get = new() { OperationId = nameof(RouteName.GetCarById) };
        OpenApiLink getAll = new() { OperationId = nameof(RouteName.GetAllCars) };
        OpenApiLink update = new() { OperationId = nameof(RouteName.UpdateCar) };
        OpenApiLink delete = new() { OperationId = nameof(RouteName.DeleteCarById) };

        get.Parameters.Add("id", new RuntimeExpressionAnyWrapper { Any = new OpenApiString("$response.body#/id") });
        get.Description = $"The `id` value returned in the response can be used as the `id` parameter in `PUT {_routePrefix}/{{id}}` or `DELETE {_routePrefix}/{{id}}`";

        update.Parameters.Add("id", new RuntimeExpressionAnyWrapper { Any = new OpenApiString("$response.body#/id") });
        //update.RequestBody = new RuntimeExpressionAnyWrapper { Any = new OpenApiSchema( ??? reference request body for PUT as a schema
        update.Description = $"The `id` value returned in the response can be used as the `id` parameter in `GET {_routePrefix}/{{id}}` or `DELETE {_routePrefix}/{{id}}`";

        delete.Parameters.Add("id", new RuntimeExpressionAnyWrapper { Any = new OpenApiString("$response.body#/id") });
        delete.Description = $"Deletes the entity matching `id` parameter if it exists";


        return new Dictionary<RouteName, OpenApiLink>
        {
            { RouteName.AddCar, add },
            { RouteName.GetCarById, get },
            { RouteName.GetAllCars, getAll },
            { RouteName.UpdateCar, update },
            { RouteName.DeleteCarById, delete },
        };
    }
    private static Dictionary<RouteName, string> BuildDescriptionsByName()
    {
        return new Dictionary<RouteName, string>
        {
            { RouteName.AddCar, "Adds a new car" },
            { RouteName.GetCarById, "Retrieves a car with id matching provided `id`" },
            { RouteName.GetAllCars, "Retrieves a summary of all cars" },
            { RouteName.UpdateCar, "updates an existing car with id matching provided `id`" },
            { RouteName.DeleteCarById, "Deletes an existing car with id matching provided `id`" },
        };
    }
}
