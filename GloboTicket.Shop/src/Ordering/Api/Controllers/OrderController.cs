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

using System.Net.Mime;
using GloboTicket.Shop.Ordering.Api.Models;
using GloboTicket.Shop.Ordering.Application.Features.Orders.Command.CreateOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GloboTicket.Shop.Ordering.Api.Controllers;

[Route("api/orders")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <inheritdoc />
    public OrderController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Submit a new order
    /// </summary>
    [HttpPost(Name = nameof(SubmitOrder))]
    public async Task<IActionResult> SubmitOrder([FromBody] OrderForCreation orderDto, CancellationToken cancellationToken)
    {
        // TODO: rework this so it's not mapping one dto to another, either have a shared library or maybe generated from swagger?
        await _mediator.Send(new CreateOrderCommand(orderDto.ToCreateOrderDto()), cancellationToken);

        // TODO: provide get route and use alongside CreatedAtRoute
        return new StatusCodeResult(StatusCodes.Status201Created);
    }
}
