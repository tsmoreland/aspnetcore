using CarInventory.Application.Features.Cars.Shared;
using MediatR;

namespace CarInventory.Application.Features.Cars.Queries.GetAll;

public sealed record class GetAllQuery()  : IStreamRequest<CarDetails>;
