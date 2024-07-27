using System.Text;

namespace MicroShop.Services.Email.App.Models.DataTransferObjects;

public sealed class EmailMessage
{
    public string Name { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public CartSummaryDto Content { get; set; } = CartSummaryDto.Empty();

    public EmailLogEntry ToLogCartEntry()
    {
        StringBuilder builder = new();
        builder.Append($"""
            <br/>
            Total: {Content.CartTotal}<br/>
            <ul>
            """);

        foreach (CartItemDto item in Content.Details)
        {
            builder.Append($"""
                <li>
                  {item.ProductName} x {item.Count}"
                </li>
                """);
        }
        builder.Append("</ul>");

        return new EmailLogEntry(EmailAddress, builder.ToString(), DateTime.UtcNow);
    }

}
