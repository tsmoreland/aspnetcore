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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace GloboTicket.Shop.Shared.Api.Contracts;

public interface IExceptionToProblemDetailsConverter
{
    /// <summary>
    /// May Convert <paramref name="ex"/> to an instance of <see cref="ProblemDetails"/>
    /// if supported.
    /// </summary>
    /// <param name="ex">exception to convert</param>
    /// <param name="factory">factory used to build <see cref="ProblemDetails"/></param>
    /// <param name="httpContext">HTTP Context used to populate details of the problem details</param>
    /// <returns>
    /// if supported an instance of <see cref="ProblemDetails"/>; otherwise, <see langword="null"/>
    /// </returns>
    /// <remarks>
    /// intended to be used as chain of rules (of a sort) where the first converter that can handle the conversion does
    /// It is not intended for multiple converters to be able to handle the same exception.
    /// </remarks>
    ProblemDetails? Convert(Exception ex, ProblemDetailsFactory factory, HttpContext httpContext);
}
