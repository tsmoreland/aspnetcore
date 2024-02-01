using CarInventory.Application.Features.Cars.Shared;
using MediatR;

namespace CarInventory.Application.Features.Cars.Queries.GetById;

public sealed record class GetByIdQuery(Guid Id) : IRequest<CarDetails?>;
