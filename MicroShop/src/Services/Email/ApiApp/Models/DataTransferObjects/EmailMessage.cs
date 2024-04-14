namespace MicroShop.Services.Email.ApiApp.Models.DataTransferObjects;

public sealed class EmailMessage
{
    public string Name { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public CartSummaryDto Content { get; set; } = CartSummaryDto.Empty();
}
