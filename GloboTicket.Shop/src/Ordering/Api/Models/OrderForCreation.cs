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
            CustomerName = CustomerDetails.Name,
            CustomerEmail = CustomerDetails.Email,
            BillingAddressLineOne = CustomerDetails.BillingAddressLineOne,
            BillingAddressLineTwo = CustomerDetails.BillingAddressLineTwo,
            BillingPostCode = CustomerDetails.BillingPostalCode,
            BillingCity = CustomerDetails.BillingCity,
            BillingCountry = CustomerDetails.BillingCountry,
            DeliveryAddressLineOne = CustomerDetails.DeliveryAddressLineOne,
            DeliveryAddressLineTwo = CustomerDetails.DeliveryAddressLineTwo,
            DeliveryPostCode = CustomerDetails.DeliveryPostalCode,
            DeliveryCity = CustomerDetails.DeliveryCity,
            DeliveryCountry = CustomerDetails.DeliveryCountry,
            Items = Lines.Select(li => (li.ConcertId, li.TicketCount, li.Price)),
        };
    }
}
