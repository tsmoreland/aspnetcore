namespace GloboTicket.Shop.Ordering.Api.Models;

public sealed class CustomerDetailsDto
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string BillingAddressLineOne { get; init; } = string.Empty;
    public string BillingAddressLineTwo { get; init; } = string.Empty;
    public string BillingCity { get; init; } = string.Empty;
    public string BillingCountry { get; init; } = string.Empty;
    public string BillingPostalCode { get; init; } = string.Empty;

    public string DeliveryAddressLineOne { get; init; } = string.Empty;
    public string DeliveryAddressLineTwo { get; init; } = string.Empty;
    public string DeliveryCity { get; init; } = string.Empty;
    public string DeliveryCountry { get; init; } = string.Empty;
    public string DeliveryPostalCode { get; init; } = string.Empty;

    public string CreditCardNumber { get; init; } = string.Empty;
    public string CreditCardExpiryDate { get; init; } = string.Empty;
}
