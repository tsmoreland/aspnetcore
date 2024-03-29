﻿// 
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

using GloboTicket.TicketManagement.Domain.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace GloboTicket.TicketManagement.Api.Models;

public class PageQueryParameters
{
    /// <summary>
    /// current page number to return
    /// </summary>
    /// <example>1</example>
    [SwaggerParameter("page number", Required = true)]
    public int PageNumber { get; init; }
    /// <summary>
    /// maximum number of items to return
    /// </summary>
    /// <example>10</example>
    [SwaggerParameter("page size", Required = true)]
    public int PageSize { get; init; }

    /// <summary>
    /// Create a new instance of <see cref="PageRequest"/> using the configured page number and size
    /// </summary>
    public PageRequest ToPageRequest()
    {
        return new PageRequest(PageNumber, PageSize);
    }
}
