using System.Net.Mime;
using FlightPlan.Application.Contracts.Persistence;
using FlightPlan.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlan.Api.App.Controllers;

/// <inheritdoc />
[Route("api/flight_plans")]
[ApiController]
[Authorize]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class FlightPlanController(IFlightPlanAsyncRepository repository) : ControllerBase
{
    private readonly IFlightPlanAsyncRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    /// <summary>
    /// Get Flight Plans
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>200 response with items if found; otheriwse no content</returns>
    /// <remarks>
    /// ConfigureAwait(false) isn't necessary because thea app runs without a synchronization content, it's here to silence a warning
    /// </remarks>
    [HttpGet(Name = "GetFlightPlans")]
    [Produces(MediaTypeNames.Application.Json, Type = typeof(List<FlightPlanEntity>))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FlightPlanEntity>))]
    //[SwaggerResponse(StatusCodes.Status204NoContent, "no flight plans found", contentTypes: MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll(cancellationToken).ToListAsync(cancellationToken).ConfigureAwait(false);
        return items.Count > 0
            ? Ok(items)
            : NoContent();
    }

    /// <summary>
    /// Get flight plan matching <paramref name="id"/>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetFlightPlanById")]
    //[SwaggerResponse(StatusCodes.Status200OK, "successful retrieveal of flight plan", typeof(FlightPlanEntity), contentTypes: MediaTypeNames.Application.Json)]
    //[SwaggerResponse(StatusCodes.Status404NotFound, "flight plan not found", contentTypes: MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetById([FromRoute] string id, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(id, cancellationToken).ConfigureAwait(false);
        return item is not null
            ? Ok(item)
            : NotFound();
    }

    /// <summary>
    /// Create a flight plan
    /// </summary>
    /// <param name="flightPLan"></param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns></returns>
    [HttpPost(Name = "AddFlightPlan")]
    //[SwaggerResponse(StatusCodes.Status201Created, "new flight plan added", contentTypes: MediaTypeNames.Application.Json)]
    //[SwaggerResponse(StatusCodes.Status400BadRequest, "one or more problems found with request", contentTypes: MediaTypeNames.Application.Json)]
    //[SwaggerResponse(StatusCodes.Status500InternalServerError, "a problem occurred executing request", contentTypes: MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Add([FromBody] FlightPlanEntity flightPLan, CancellationToken cancellationToken)
    {
        var (id, result) = await _repository.Add(flightPLan, cancellationToken).ConfigureAwait(false);

        return result switch
        {
            TransactionResult.Success => CreatedAtRoute("GetFlightPlanById", new { id }),
            TransactionResult.BadRequest => BadRequest(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    /// <summary>
    /// Update a flight plan
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flightPLan"></param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns></returns>
    [HttpPut("{id}", Name = "UpdateFlightPlan")]
    //[SwaggerResponse(StatusCodes.Status200OK, "successful retrieveal and update of flight plan", typeof(FlightPlanEntity), contentTypes: MediaTypeNames.Application.Json)]
    //[SwaggerResponse(StatusCodes.Status400BadRequest, "one or more problems found with request", contentTypes: MediaTypeNames.Application.Json)]
    //[SwaggerResponse(StatusCodes.Status400BadRequest, "one or more problems found with request", contentTypes: MediaTypeNames.Application.Json)]
    //[SwaggerResponse(StatusCodes.Status500InternalServerError, "a problem occurred executing request", contentTypes: MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] FlightPlanEntity flightPLan, CancellationToken cancellationToken)
    {
        var result = await _repository.Update(id, flightPLan, cancellationToken).ConfigureAwait(false);
        return result switch
        {
            TransactionResult.Success => Ok(),
            TransactionResult.NotFound => NotFound(),
            TransactionResult.BadRequest => BadRequest(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    /// <summary>
    /// Delete a flight plan
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns></returns>
    [HttpDelete("{id}", Name = "DeleteFlightPlan")]
    //[SwaggerResponse(StatusCodes.Status204NoContent, "flight plan removed", contentTypes: MediaTypeNames.Application.Json)]
    //[SwaggerResponse(StatusCodes.Status400BadRequest, "one or more problems found with request", contentTypes: MediaTypeNames.Application.Json)]
    //[SwaggerResponse(StatusCodes.Status400BadRequest, "one or more problems found with request", contentTypes: MediaTypeNames.Application.Json)]
    //[SwaggerResponse(StatusCodes.Status500InternalServerError, "a problem occurred executing request", contentTypes: MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken)
    {
        var result = await _repository.Delete(id, cancellationToken).ConfigureAwait(false);
        return result switch
        {
            TransactionResult.Success => NoContent(),
            TransactionResult.NotFound => NotFound(),
            TransactionResult.BadRequest => BadRequest(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    /// <summary>
    /// Get Flight plan department airport
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns></returns>
    [HttpGet]
    [Route("airport/departure/{id}")]
    public async Task<IActionResult> GetFlightPlanDepartmentAirport(string id, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(id, cancellationToken).ConfigureAwait(false);
        return item is not null
            ? Ok(item.DepartureAirport)
            : NotFound();
    }

    /// <summary>
    /// Get Flight Plan route
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns></returns>
    [HttpGet]
    [Route("route/{id}")]
    public async Task<IActionResult> GetFlightPlanRoute(string id, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(id, cancellationToken).ConfigureAwait(false);
        return item is not null
            ? Ok(item.Route)
            : NotFound();
    }

    /// <summary>
    /// Get Flight plan time enroute
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns></returns>
    [HttpGet]
    [Route("time/enroute/{id}")]
    public async Task<IActionResult> GetFlightPlanTimeEnroute(string id, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(id, cancellationToken).ConfigureAwait(false);
        return item is not null
            ? Ok(item.ArrivalTime - item.DepartureTime)
            : NotFound();
    }
}
