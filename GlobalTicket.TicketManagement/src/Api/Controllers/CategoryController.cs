using GlobalTicket.TicketManagement.Api.Models;
using GlobalTicket.TicketManagement.Application.Features.Categories.Commands.CreateCategory;
using GlobalTicket.TicketManagement.Application.Features.Categories.Queries.GetCatagoriesPage;
using GlobalTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesPageWithEvents;
using GlobalTicket.TicketManagement.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GlobalTicket.TicketManagement.Api.Controllers;

[Route("api/categories")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    private static class RouteNames
    {
        public const string GetPage = "GetCategories";
        public const string Create = "AddCategory";

    }

    /// <inheritdoc />
    public CategoryController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet(Name = RouteNames.GetPage)]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        CreateCategoryCommandResponse response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
}
