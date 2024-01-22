using System.Net.Mime;
using GloboTicket.TicketManagement.Api.Models;
using GloboTicket.TicketManagement.Application.Features.Categories.Commands.CreateCategory;
using GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCatagoriesPage;
using GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesPageWithEvents;
using GloboTicket.TicketManagement.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GloboTicket.TicketManagement.Api.Controllers;

/// <inheritdoc />
[Route("api/categories")]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails), ContentTypes = new[] { "applicaiton/problem+json" })]
public class CategoryController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    private static class RouteNames
    {
        public const string GetPage = "GetCategories";
        public const string Create = "AddCategory";

    }

    [HttpGet(Name = RouteNames.GetPage)]
    [SwaggerOperation("Gets categories in pages, optionally including events", OperationId = RouteNames.GetPage)]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Page<CategoryWithEventsViewModel>), ContentTypes = new[] { MediaTypeNames.Application.Json })]
    public async Task<IActionResult> GetCategoriesPage([FromQuery] CategoryPageQueryParameters parameters, CancellationToken cancellationToken)
    {
        if (parameters.IncludeEvents)
        {
            Page<CategoryWithEventsViewModel> viewModels = await _mediator.Send(new GetCategoriesPageWithEventsQuery(parameters.ToPageRequest(), true), cancellationToken);
            return Ok(viewModels);
        }
        else
        {
            Page<CategoryViewModel> viewModels = await _mediator.Send(new GetCategoriesPageQuery(parameters.ToPageRequest()), cancellationToken);
            return Ok(viewModels);
        }
    }

    [HttpPost(Name = RouteNames.Create)]
    [SwaggerOperation("Creates a new category", OperationId = RouteNames.Create)]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(CreateCategoryCommandResponse), ContentTypes = new[] { MediaTypeNames.Application.Json })]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        CreateCategoryCommandResponse response = await _mediator.Send(command, cancellationToken);
        return new ObjectResult(response) { StatusCode = StatusCodes.Status201Created };
    }
}
