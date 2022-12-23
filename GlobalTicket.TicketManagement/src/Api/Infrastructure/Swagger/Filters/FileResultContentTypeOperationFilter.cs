using GlobalTicket.TicketManagement.Api.Infrastructure.Swagger.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GlobalTicket.TicketManagement.Api.Infrastructure.Swagger.Filters;

public sealed class FileResultContentTypeOperationFilter : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var requestAttribute = context.MethodInfo.GetCustomAttributes(typeof(FileResultContentTypeAttribute), false)
            .Cast<FileResultContentTypeAttribute>()
            .FirstOrDefault();

        if (requestAttribute == null) return;

        operation.Responses.Clear();
        operation.Responses.Add("200", new OpenApiResponse
        {
            Content = new Dictionary<string, OpenApiMediaType>
            {
                [requestAttribute.ContentType] =
                    new()
                    {
                        Schema = new OpenApiSchema { Type = "string", Format = "binary" }
                    },
            }
        });
    }
}
