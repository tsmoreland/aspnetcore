using CarInventory.Cars.Application.Features.Shared;
using MediatR;

namespace CarInventory.Cars.Application.Features.Queries.GetAll;

public sealed record class GetAllQuery() : IStreamRequest<CarDetails>;
