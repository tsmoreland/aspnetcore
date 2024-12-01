using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace FlightPlan.Api.App.Infrastructure;

/// <summary>
/// A filter that specifies the expected <see cref="ProblemDetails"/> the action will return and 'application/problem+json'
/// response content type. 
/// </summary>
/// <param name="statusCode">The error status code described by the response</param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class ProducesProblemAttribute(int statusCode) : Attribute, IResultFilter, IOrderedFilter, IApiResponseMetadataProvider
{
    /// <inheritdoc />
    public void OnResultExecuting(ResultExecutingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        if (context.Result is ObjectResult objectResult) SetContentTypes(objectResult.ContentTypes);
    }

    /// <inheritdoc />
    public void OnResultExecuted(ResultExecutedContext context)
    {
        _ = context;
    }

    /// <inheritdoc />
    public int Order { get; set; }

    /// <inheritdoc />
    public void SetContentTypes(MediaTypeCollection contentTypes)
    {
        contentTypes.Clear();
        contentTypes.Add("application/problem+json");
    }

    /// <inheritdoc />
    public Type? Type => typeof(ProblemDetails);

    /// <inheritdoc />
    public int StatusCode => statusCode;
}
