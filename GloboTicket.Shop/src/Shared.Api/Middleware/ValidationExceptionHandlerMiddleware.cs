//
// Copyright © 2023 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Diagnostics;
using System.Runtime.ExceptionServices;
using GloboTicket.Shop.Shared.Api.Contracts;
using GloboTicket.Shop.Shared.Api.Extensions;
using GloboTicket.Shop.Shared.Api.Models;
using GloboTicket.Shop.Shared.Contracts.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

namespace GloboTicket.Shop.Shared.Api.Middleware;

/// <summary/>
public sealed class ValidationExceptionHandlerMiddleware
{

    private readonly RequestDelegate _next;
    private readonly IEnumerable<IExceptionToProblemDetailsConverter> _converters;

    /// <summary/>
    public ValidationExceptionHandlerMiddleware(RequestDelegate next, IEnumerable<IExceptionToProblemDetailsConverter> converters)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _converters = converters ?? throw new ArgumentNullException(nameof(converters));
    }

    /// <summary>
    /// Intercepts and translates exceptions to Problem details response 
    /// </summary>
    public Task Invoke(HttpContext context)
    {
        Task task = _next.Invoke(context);
        return !task.IsCompletedSuccessfully
            ? Awaited(context, task)
            : Task.CompletedTask;

    }

    private async Task Awaited(HttpContext context, Task task)
    {
        ExceptionDispatchInfo? edi = null;
        try
        {
            if (!task.IsCompletedSuccessfully)
            {
                await task;
            }
        }
        catch (Exception ex)
        {
            edi = ExceptionDispatchInfo.Capture(ex);
        }

        if (edi is not null)
        {
            await HandleError(context, edi);
        }

    }

    private async ValueTask HandleError(HttpContext context, ExceptionDispatchInfo edi)
    {
        // Log error
        if (context.Response.HasStarted)
        {
            edi.Throw();
        }

        ProblemDetailsFactory problemDetailsFactory = context.RequestServices.GetRequiredService<ProblemDetailsFactory>();
        DiagnosticListener? diagnosticListener = context.RequestServices.GetService<DiagnosticListener>();

        if (diagnosticListener is not null && diagnosticListener.IsEnabled() && diagnosticListener.IsEnabled("Microsoft.AspNetCore.Diagnostics.HandledException"))
        {
            diagnosticListener.Write("Microsoft.AspNetCore.Diagnostics.HandledException", new { httpContext = context, exception = edi.SourceException });
        }

        ModelStateDictionary? modelState = context.Features.Get<ModelStateContainer>()?.ModelState;

        ProblemDetails? problem = edi.SourceException switch
        {
            BadRequestException e => e.ToProblemDetails(problemDetailsFactory, context),
            NotFoundException e => e.ToProblemDetails(problemDetailsFactory, context),
            ValidationFailureException e => e.ToProblemDetails(problemDetailsFactory, context, modelState),
            _ => TryConvert(edi.SourceException, problemDetailsFactory, context),
        };
        if (problem is null)
        {
            edi.Throw(); // not one of our known exceptions, forward on to default exception middleware
        }

        context.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problem);
    }

    private ProblemDetails? TryConvert(Exception ex, ProblemDetailsFactory factory, HttpContext httpContext)
    {
        return _converters
            .Select(c => c.Convert(ex, factory, httpContext))
            .FirstOrDefault(pd => pd is not null);
    }
}
