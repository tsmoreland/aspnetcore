using System.ComponentModel.DataAnnotations;

namespace GloboTicket.FrontEnd.Mvc.App.Models.View;

public class CheckoutViewModel
{
    public Guid BasketId { get; init; }
    public string Name { get; init; } = string.Empty;

    public string BillingAddressLineOne { get; init; } = string.Empty;
    public string BillingAddressLineTwo { get; init; } = string.Empty;
    public string BillingPostalCode { get; init; } = string.Empty;
    public string BillingCity { get; init; } = string.Empty;
    public string BillingCountry { get; init; } = string.Empty;

    public string DeliveryAddressLineOne { get; init; } = string.Empty;
    public string DeliveryAddressLineTwo { get; init; } = string.Empty;
    public string DeliveryPostalCode { get; init; } = string.Empty;
    public string DeliveryCity { get; init; } = string.Empty;
    public string DeliveryCountry { get; init; } = string.Empty;

    [EmailAddress]
    public string Email { get; init; } = string.Empty;
    [CreditCard]

    public string CreditCard { get; init; } = string.Empty;
    public string CreditCardDate { get; init; } = string.Empty;
}
