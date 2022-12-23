using GlobalTicket.TicketManagement.Api.Models;
using GlobalTicket.TicketManagement.Api.Models.Events;
using GlobalTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;
using GlobalTicket.TicketManagement.Application.Features.Events.Commands.DeleteEvent;
using GlobalTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail;
using GlobalTicket.TicketManagement.Application.Features.Events.Queries.GetEventsPage;
using GlobalTicket.TicketManagement.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GlobalTicket.TicketManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
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
    };

    /// <inheritdoc />
    public EventsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet(Name = RouteNames.GetPage)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetPage([FromQuery] PageQueryParameters parameters, CancellationToken cancellationToken)
    {
        Page<EventViewModel> events = await _mediator.Send(new GetEventsPageQuery(parameters.ToPageRequest()), cancellationToken);
        return Ok(events);
    }

    [HttpGet("{id}", Name = RouteNames.GetById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEventById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        EventDetailViewModel? maybeEvent = await _mediator.Send(new GetEventDetailQuery(id), cancellationToken);
        return maybeEvent is not null
            ? Ok(maybeEvent)
            : NotFound();

    }

    [HttpPost(Name = RouteNames.Create)]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventCommand command, CancellationToken cancellationToken)
    {
        Guid id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(RouteNames.GetById, new {id}, cancellationToken);
    }

    [HttpPut("{id}", Name = RouteNames.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEventDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(dto.ToCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpPut("{id}", Name = RouteNames.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteEventCommand(id), cancellationToken);
        return NoContent();
    }
}
