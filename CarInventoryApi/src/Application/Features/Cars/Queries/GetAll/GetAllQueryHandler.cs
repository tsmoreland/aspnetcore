using System.Runtime.CompilerServices;
using CarInventory.Application.Features.Cars.Shared;
using CarInventory.Domain.Contracts;
using CarInventory.Domain.Models;
using MediatR;

namespace CarInventory.Application.Features.Cars.Queries.GetAll;

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
