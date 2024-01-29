using CarInventory.Application.Features.Cars.Shared;
using CarInventory.Domain.Contracts;
using CarInventory.Domain.Models;
using MediatR;

namespace CarInventory.Application.Features.Cars.Queries.GetById;

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
