using System.Net.Mime;
using GloboTicket.TicketManagement.Api.Infrastructure.Swagger.Attributes;
using GloboTicket.TicketManagement.Api.Models;
using GloboTicket.TicketManagement.Api.Models.Events;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.DeleteEvent;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsExport;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsPage;
using GloboTicket.TicketManagement.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GloboTicket.TicketManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails), ContentTypes = new[] { "applicaiton/problem+json" })]
public class EventsController : ControllerBase
{
    private readonly IMediator _mediator;

    private static class RouteNames
    {
        public const string GetPage = "GetEvents";
        public const string GetById = "GetEventById";
        public const string Create = "AddEvent";
        public const string Update = "UpdateEvent";
        public const string Delete = "DeleteEvent";
        public const string Export = "ExportEvents";
    };

    /// <inheritdoc />
    public EventsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet(Name = RouteNames.GetPage)]
    [SwaggerOperation("Gets a page of Event View Models", OperationId = RouteNames.GetPage)]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Page<EventViewModel>), ContentTypes = new[] { MediaTypeNames.Application.Json })]
    public async Task<IActionResult> GetPage([FromQuery] PageQueryParameters parameters, CancellationToken cancellationToken)
    {
        Page<EventViewModel> events = await _mediator.Send(new GetEventsPageQuery(parameters.ToPageRequest()), cancellationToken);
        return Ok(events);
    }

    [HttpGet("{id}", Name = RouteNames.GetById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation("Gets an Event View Models by id", OperationId = RouteNames.GetById)]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(EventDetailViewModel), ContentTypes = new[] { MediaTypeNames.Application.Json })]
    [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails), ContentTypes = new[] { "applicaiton/problem+json" })]
    public async Task<IActionResult> GetEventById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        EventDetailViewModel? maybeEvent = await _mediator.Send(new GetEventDetailQuery(id), cancellationToken);
        return maybeEvent is not null
            ? Ok(maybeEvent)
            : NotFound();

    }

    [HttpPost(Name = RouteNames.Create)]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(EventViewModel), ContentTypes = new[] { MediaTypeNames.Application.Json })]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventCommand command, CancellationToken cancellationToken)
    {
        Guid id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(RouteNames.GetById, new { id }, cancellationToken);
    }

    [HttpPut("{id}", Name = RouteNames.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails), ContentTypes = new[] { "applicaiton/problem+json" })]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEventDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(dto.ToCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}", Name = RouteNames.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails), ContentTypes = new[] { "applicaiton/problem+json" })]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteEventCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpGet("export", Name = RouteNames.Export)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    [FileResultContentType("text/csv")]
    public async Task<FileResult> ExportEvents()
    {
        EventExportFileViewModel fileDto = await _mediator.Send(new GetEventsExportQuery());

        return File(fileDto.Data, fileDto.ContentType, fileDto.EventExportFilename);
    }
}
