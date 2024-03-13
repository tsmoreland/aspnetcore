using CarInventory.Cars.Application.Features.Shared;
using MediatR;

namespace CarInventory.Cars.Application.Features.Queries.GetById;

public sealed record class GetByIdQuery(Guid Id) : IRequest<CarDetails?>;
