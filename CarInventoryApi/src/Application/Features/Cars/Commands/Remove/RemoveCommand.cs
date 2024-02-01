using MediatR;

namespace CarInventory.Application.Features.Cars.Commands.Remove;

public sealed record class RemoveCommand(Guid Id) : IRequest;
