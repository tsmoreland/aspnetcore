using GloboTicket.Shop.Catalog.Domain.Models;
using GloboTicket.Shop.Shared.Contracts.Persistence;

namespace GloboTicket.Shop.Catalog.Domain.Contracts.Persistence;

public interface IConcertRepository : IAsyncRepository<Concert>
{
}
