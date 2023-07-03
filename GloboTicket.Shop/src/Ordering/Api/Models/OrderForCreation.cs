using GloboTicket.Shop.Ordering.Application.Features.Orders.Command.CreateOrder;
using GloboTicket.Shop.Ordering.Domain.Models;

namespace GloboTicket.Shop.Ordering.Api.Models;

public class OrderForCreation
{
    public required Guid OrderId { get; set; }
    public required DateTimeOffset Date { get; set; }
    public required CustomerDetailsDto CustomerDetails { get; set; }
    public required List<OrderLine> Lines { get; set; }

    public CreateOrderDto ToCreateOrderDto()
    {
        return new CreateOrderDto
        {
            Date = Date,
            CustomerDetails = CustomerDetails.ToCommandDto(),
            Items = Lines.Select(li => (li.ConcertId, li.TicketCount, li.Price)),
        };
    }
}
