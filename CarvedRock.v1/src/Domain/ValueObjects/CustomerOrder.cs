namespace CarvedRock.Domain.ValueObjects;

public sealed record class CustomerOrder(int OrderId, string OrderName, int CustomerId, string CustomerName, string ItemDescription, decimal Price, decimal PriceAfterVat);
