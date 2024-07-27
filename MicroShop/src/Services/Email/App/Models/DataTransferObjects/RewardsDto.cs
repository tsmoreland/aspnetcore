using System.Text;

namespace MicroShop.Services.Email.App.Models.DataTransferObjects;

public sealed record class RewardsDto(string UserId, int RewardsActivity, int OrderId)
{
    public EmailLogEntry ToLogOrderCreatedEntry(string emailAddress)
    {
        StringBuilder builder = new();
        builder.Append($"""
            <br/>
            Order: {OrderId}<br?>
            Reward Points: {RewardsActivity}
            <ul>
            """);

        return new EmailLogEntry(emailAddress, builder.ToString(), DateTime.UtcNow);
    }
}
