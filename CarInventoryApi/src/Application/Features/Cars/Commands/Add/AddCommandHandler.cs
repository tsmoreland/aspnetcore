﻿using CarInventory.Application.Features.Cars.Shared;
using CarInventory.Domain.Contracts;
using CarInventory.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarInventory.Application.Features.Cars.Commands.Add;

public sealed class AddCommandHandler(ICarRepository repository, ILogger<AddCommandHandler> logger) : IRequestHandler<AddCommand, CarDetails>
{
    private readonly ICarRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<AddCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public async Task<CarDetails> Handle(AddCommand request, CancellationToken cancellationToken)
    {
        Car car = _repository.Add(request.Make, request.Model, request.HorsePower, request.EngineType, request.FuelCapacityInLitres, request.NumberOfDoors, request.MpG);
        await _repository.SaveChanges(cancellationToken);
        return new CarDetails(car);
    }
}
