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

using System.Transactions;
using FlightPlan.Application.Contracts.Persistence;
using FlightPlan.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlan.Api.App.Controllers
{
    [Route("api/flight_plans")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {
        private readonly IFlightPlanAsyncRepository _repository;

        /// <inheritdoc />
        public FlightPlanController(IFlightPlanAsyncRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet(Name = "GetFlightPlans")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            List<FlightPlanEntity> items = await _repository.GetAll(cancellationToken).ToListAsync(cancellationToken);
            return items.Count > 0
                ? Ok(items)
                : NoContent();
        }

        [HttpGet("{id}", Name = "GetFlightPlanById")]
        public async Task<IActionResult> GetById([FromRoute] string id, CancellationToken cancellationToken)
        {
            FlightPlanEntity? item = await _repository.GetById(id, cancellationToken);
            return item is not null
                ? Ok(item)
                : NotFound();
        }

        [HttpPost(Name = "AddFlightPlan")]
        public async Task<IActionResult> Add([FromBody] FlightPlanEntity flightPLan, CancellationToken cancellationToken)
        {
            (string id, TransactionResult result) = await _repository.Add(flightPLan, cancellationToken);

            return result switch
            {
                TransactionResult.Success =>  CreatedAtRoute("GetFlightPlanById", new { id }),
                TransactionResult.BadRequest => BadRequest(),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        [HttpPut("{id}", Name = "UpdateFlightPlan")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] FlightPlanEntity flightPLan, CancellationToken cancellationToken)
        {
            TransactionResult result = await _repository.Update(id, flightPLan, cancellationToken);
            return result switch
            {
                TransactionResult.Success =>  Ok(),
                TransactionResult.NotFound => NotFound(),
                TransactionResult.BadRequest => BadRequest(),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        [HttpDelete("{id}", Name = "DeleteFlightPlan")]
        public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken)
        {
            TransactionResult result = await _repository.Delete(id, cancellationToken);
            return result switch
            {
                TransactionResult.Success =>  Ok(),
                TransactionResult.NotFound => NotFound(),
                TransactionResult.BadRequest => BadRequest(),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        [HttpGet]
        [Route("airport/departure/{id}")]
        public async Task<IActionResult> GetFlightPlanDepartmentAirport(string id, CancellationToken cancellationToken)
        {
            FlightPlanEntity? item = await _repository.GetById(id, cancellationToken);
            return item is not null
                ? Ok(item.DepartureAirport)
                : NotFound();
        }

        [HttpGet]
        [Route("route/{id}")]
        public async Task<IActionResult> GetFlightPlanRoute(string id, CancellationToken cancellationToken)
        {
            FlightPlanEntity? item = await _repository.GetById(id, cancellationToken);
            return item is not null
                ? Ok(item.Route)
                : NotFound();
        }

        [HttpGet]
        [Route("time/enroute/{id}")]
        public async Task<IActionResult> GetFlightPlanTimeEnroute(string id, CancellationToken cancellationToken)
        {
            FlightPlanEntity? item = await _repository.GetById(id, cancellationToken);
            return item is not null
                ? Ok(item.ArrivalTime - item.DepartureTime)
                : NotFound();
        }
    }
}
