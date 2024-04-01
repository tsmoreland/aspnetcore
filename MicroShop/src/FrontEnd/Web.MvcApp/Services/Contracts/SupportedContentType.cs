using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.FileProviders.Composite;

namespace MicroShop.Web.MvcApp.Services.Contracts;

public enum SupportedContentType
{
    ApplicationJson,
    MultiPartForm,
}

internal static class SupportedContentTypeExtensions
{
    public static string ToMediaTypeName(this SupportedContentType contentType)
    {
        return contentType switch
        {
            SupportedContentType.ApplicationJson => MediaTypeNames.Application.Json,
            SupportedContentType.MultiPartForm => MediaTypeNames.Multipart.FormData,
            _ => throw new KeyNotFoundException("Unrecognized content type")
        };
    }

    public static string ToAcceptMediaTypeName(this SupportedContentType contentType)
    {
        return contentType switch
        {
            SupportedContentType.ApplicationJson => MediaTypeNames.Application.Json,
            SupportedContentType.MultiPartForm => "*/*",
            _ => throw new KeyNotFoundException("Unrecognized content type")
        };
    }

    [return: NotNullIfNotNull(nameof(data))]
    public static HttpContent? BuildHttpContent(this SupportedContentType contentType, object? data)
    {
        if (data is null)
        {
            return null;
        }

        return contentType switch
        {
            // not he most efficient way to build json content (stream may be preferable or one of the may AsJson like methods availble to HttpClient but this will do for now)
            SupportedContentType.ApplicationJson => new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, MediaTypeNames.Application.Json),
            SupportedContentType.MultiPartForm => BuildFormContent(data),
            _ => throw new KeyNotFoundException("Unrecognized content type")
        };

        static MultipartFormDataContent BuildFormContent(object data)
        {
            MultipartFormDataContent content = [];
            foreach (PropertyInfo property in data.GetType().GetProperties())
            {
                object? value = property.GetValue(data);
                if (IsNullOrDefault(value))
                {
                    continue;
                }

                if (value is IFormFile file)
                {
                    content.Add(new StreamContent(file.OpenReadStream()), property.Name, file.FileName);
                }
                else if (value?.ToString() is { } contentValue)
                {
                    content.Add(new StringContent(contentValue), property.Name);
                }
            }

            return content;

            static bool IsNullOrDefault(object? value)
            {
                if (value is null)
                {
                    return true;
                }

                Type type = value.GetType();
                return type.IsValueType && Activator.CreateInstance(type)?.Equals(value) == true;
            }
        }
    }
}
