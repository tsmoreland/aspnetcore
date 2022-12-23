namespace GlobalTicket.TicketManagement.Api.Infrastructure.Swagger.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class FileResultContentTypeAttribute : Attribute
{
    /// <inheritdoc />
    public FileResultContentTypeAttribute(string contentType)
    {
        ContentType = contentType;
    }

    public string ContentType { get; }
}
