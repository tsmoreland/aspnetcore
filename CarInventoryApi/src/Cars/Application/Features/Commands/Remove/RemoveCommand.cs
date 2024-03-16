using MediatR;

namespace CarInventory.Cars.Application.Features.Commands.Remove;

public sealed record class RemoveCommand(Guid Id) : IRequest;
