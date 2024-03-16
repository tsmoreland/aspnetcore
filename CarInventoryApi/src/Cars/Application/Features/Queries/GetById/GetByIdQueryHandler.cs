using CarInventory.Cars.Application.Features.Shared;
using CarInventory.Cars.Domain.Contracts;
using CarInventory.Cars.Domain.Models;
using MediatR;

namespace CarInventory.Cars.Application.Features.Queries.GetById;

public sealed class GetByIdQueryHandler(ICarRepository repository) : IRequestHandler<GetByIdQuery, CarDetails?>
{
    private readonly ICarRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    /// <inheritdoc />
    public async Task<CarDetails?> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        Car? car = await _repository.GetCarById(request.Id, cancellationToken);
        return car is not null
            ? new CarDetails(car)
            : null;
    }
}
