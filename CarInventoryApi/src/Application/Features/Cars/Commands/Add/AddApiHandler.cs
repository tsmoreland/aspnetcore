//
// Copyright © 2023 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using CarInventory.Application.Features.Cars.Shared;
using CarInventory.Domain.Contracts;
using CarInventory.Domain.Models;
using CarInventory.Shared;
using CarInventory.Shared.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace CarInventory.Application.Features.Cars.Commands.Add;

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
            // TODO: translate Validation Exception to problem details, ideally via some other class (maybe extension method)
            return TypedResults.BadRequest(ex.ToProblemDetails(httpContext));
        }
    }
}
