namespace MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Requests;

public sealed record class CreateOrderDto(string Name, string EmailAddress, CartSummaryDto Cart);
