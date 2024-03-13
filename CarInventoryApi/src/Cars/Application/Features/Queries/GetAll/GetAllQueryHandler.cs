using System.Runtime.CompilerServices;
using CarInventory.Cars.Application.Features.Shared;
using CarInventory.Cars.Domain.Contracts;
using CarInventory.Cars.Domain.Models;
using MediatR;

namespace CarInventory.Cars.Application.Features.Queries.GetAll;

public sealed class GetAllQueryHandler(ICarRepository repository) : IStreamRequestHandler<GetAllQuery, CarDetails>
{
    private readonly ICarRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    /// <inheritdoc />
    public async IAsyncEnumerable<CarDetails> Handle(GetAllQuery request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (Car car in _repository.GetAllCars().WithCancellation(cancellationToken))
        {
            yield return new CarDetails(car);
        }
    }
}
