using GloboTicket.FrontEnd.Mvc.App.Models.Api;
using GloboTicket.FrontEnd.Mvc.App.Models.View;
using GloboTicket.FrontEnd.Mvc.App.Services.ShoppingBasket;

namespace GloboTicket.FrontEnd.Mvc.App.Services.Ordering;

public sealed class HttpOrderSubmissionService : IOrderSubmissionService
{
    private readonly IShoppingBasketService _shoppingBasketService;
    private readonly HttpClient _orderingClient;

    public HttpOrderSubmissionService(IShoppingBasketService shoppingBasketService, HttpClient orderingClient)
    {
        _shoppingBasketService = shoppingBasketService ?? throw new ArgumentNullException(nameof(shoppingBasketService));
        _orderingClient = orderingClient ?? throw new ArgumentNullException(nameof(orderingClient));
    }
    public async Task<Guid> SubmitOrder(CheckoutViewModel checkoutViewModel, CancellationToken cancellationToken)
    {
        IAsyncEnumerable<BasketLine> lines = _shoppingBasketService
            .GetLinesForBasket(checkoutViewModel.BasketId, cancellationToken);
        List<OrderLine> orderLines = await lines
            .Select(line => new OrderLine()
            {
                ConcertId = line.ConcertId,
                Price = line.Price,
                TicketCount = line.TicketAmount
            })
            .ToListAsync(cancellationToken);

        OrderForCreation order = new()
        {
            Date = DateTimeOffset.Now,
            OrderId = Guid.NewGuid(),
            Lines = orderLines,
            CustomerDetails = new CustomerDetails()
            {
                Name = checkoutViewModel.Name,
                Email = checkoutViewModel.Email,
                BillingAddressLineOne = checkoutViewModel.BillingAddressLineOne,
                BillingAddressLineTwo = checkoutViewModel.BillingAddressLineTwo,
                BillingCity = checkoutViewModel.BillingCity,
                BillingCountry = checkoutViewModel.BillingCountry,
                BillingPostalCode = checkoutViewModel.BillingPostalCode,
                DeliveryAddressLineOne = checkoutViewModel.DeliveryAddressLineOne,
                DeliveryAddressLineTwo = checkoutViewModel.DeliveryAddressLineTwo,
                DeliveryCity = checkoutViewModel.DeliveryCity,
                DeliveryCountry = checkoutViewModel.DeliveryCountry,
                DeliveryPostalCode = checkoutViewModel.DeliveryPostalCode,
                CreditCardNumber = checkoutViewModel.CreditCard,
                CreditCardExpiryDate = checkoutViewModel.CreditCardDate
            },
        };
        // make a synchronous call to the ordering microservice
        HttpResponseMessage response = await _orderingClient.PostAsJsonAsync("orders", order);
        if (response.IsSuccessStatusCode)
        {
            return order.OrderId;
        }
        else
        {
            throw new ApplicationException("TODO: Handle Validation error");
        }
    }
}
