// 
// Copyright © 2022 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using GloboTicket.TicketManagement.Domain.Contracts.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GloboTicket.TicketManagement.Api.Models;

public static class ExceptionExtensions
{
    public static ProblemDetails ToProblemDetails(this ValidationFailureException ex, ProblemDetailsFactory problemDetailsFactory, HttpContext context, ModelStateDictionary? modelState)
    {
        if (modelState is null)
        {
            return problemDetailsFactory.CreateProblemDetails(context, StatusCodes.Status400BadRequest, title: "One or more properties was invalid", detail: ex.Message);
        }

        foreach ((string property, string errorMessage) in ex.ValidationErrors)
        {
            modelState.AddModelError(property, errorMessage);
        }

        return problemDetailsFactory
            .CreateValidationProblemDetails(context, modelState, StatusCodes.Status400BadRequest, title: "One or more properties was invalid", detail: ex.Message);
    }

    public static ProblemDetails ToProblemDetails(this NotFoundException ex, ProblemDetailsFactory problemDetailsFactory, HttpContext context)
    {
        return problemDetailsFactory.CreateProblemDetails(context, StatusCodes.Status404NotFound, title: "Not Found", detail: ex.Message);
    }
    public static ProblemDetails ToProblemDetails(this BadRequestException ex, ProblemDetailsFactory problemDetailsFactory, HttpContext context)
    {
        return problemDetailsFactory.CreateProblemDetails(context, StatusCodes.Status400BadRequest, title: "One or more arguments are invalid", detail: ex.Message);
    }

    public static ProblemDetails ToProblemDetails(this IdentityException ex, ProblemDetailsFactory problemDetailsFactory, HttpContext context)
    {
        int statusCode;
        string title;
        if (context.User.Identity is not null)
        {
            statusCode = StatusCodes.Status403Forbidden;
            title = "not authorized for this request";
        }
        else
        {
            statusCode = StatusCodes.Status401Unauthorized;
            title = "endpoint requires authentication";
        }

        string? detail = ex.GetUserFriendlyDetail();
        return problemDetailsFactory.CreateProblemDetails(context, statusCode, title, detail);
    }
}
