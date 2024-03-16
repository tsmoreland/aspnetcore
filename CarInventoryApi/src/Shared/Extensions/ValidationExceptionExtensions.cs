using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarInventory.Shared.Extensions;

public static class ValidationExceptionExtensions
{
    public static ProblemDetails ToProblemDetails(this FluentValidation.ValidationException exception,
        string? detail = null,
        string? instance = null,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        IDictionary<string, object?>? extensions = null)
    {
        HttpValidationProblemDetails problemDetails = new(exception.ToDictionary())
        {
            Detail = detail,
            Instance = instance,
            Type = type,
            Status = statusCode,
        };
        problemDetails.Title ??= title;

        CopyExtensions(problemDetails, extensions);
        return problemDetails;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyExtensions(ProblemDetails problemDetails, IDictionary<string, object?>? extensions)
    {
        if (extensions is null)
        {
            return;
        }

        foreach (KeyValuePair<string, object?> extension in extensions)
        {
            problemDetails.Extensions.Add(extension);
        }
    }
    private static Dictionary<string, string[]> ToDictionary(this FluentValidation.ValidationException exception)
    {
        return exception.Errors
            .GroupBy(static e => e.PropertyName)
            .ToDictionary(
                static g => g.Key,
                static g => g.Select(e => e.ErrorMessage).ToArray());
    }
}
