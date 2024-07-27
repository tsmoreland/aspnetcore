namespace MicroShop.Services.Orders.App.Models.DataTransferObjects.Requests;

public sealed record class CreateOrderDto(string Name, string EmailAddress, CartSummaryDto Cart);
