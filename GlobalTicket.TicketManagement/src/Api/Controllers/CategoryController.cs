//
// Copyright © 2022 Terry Moreland
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

using GlobalTicket.TicketManagement.Api.Models;
using GlobalTicket.TicketManagement.Application.Features.Categories.Commands.CreateCategory;
using GlobalTicket.TicketManagement.Application.Features.Categories.Queries.GetCatagoriesPage;
using GlobalTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesPageWithEvents;
using GlobalTicket.TicketManagement.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GlobalTicket.TicketManagement.Api.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        private static class RouteNames
        {
            public const string GetCategoriesPage = "GetCategories";
            public const string CreateCategory = "AddCategory";

        }

        /// <inheritdoc />
        public CategoryController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet(Name = RouteNames.GetCategoriesPage)]
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

        [HttpPost(Name = RouteNames.CreateCategory)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            CreateCategoryCommandResponse response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }
    }
}
