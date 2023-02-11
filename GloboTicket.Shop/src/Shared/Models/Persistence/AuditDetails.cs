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

using System.Runtime.CompilerServices;

namespace GloboTicket.Shop.Shared.Models.Persistence;

public sealed record class AuditDetails(string? CreatedBy, DateTime CreatedDate, string? LastModifiedBy, DateTime LastModifiedDate)
{
    public static readonly int MaxCreatedByLength = 200;
    public static readonly int MaxLastModifiedByLength = 200;

    public void ThrowIfNotValid(string? parameterName = null, string? message = null)
    {
        if (!IsValid())
        {
            throw new ArgumentNullException(parameterName, message);
        }
    }

    public void ThrowIfCannotTransition(AuditDetails proposed, [CallerArgumentExpression("proposed")] string? parameterName = null, string? message = null)
    {
        if (!CanTransitionTo(proposed))
        {
            throw new ArgumentNullException(parameterName, message);
        }
    }

    public bool IsValid()
    {
        if (CreatedBy is not { Length: > 0 } || CreatedBy.Length > MaxCreatedByLength)
        {
            return false;
        }

        if (CreatedDate == DateTime.MinValue)
        {
            return false;
        }

        return true;
    }

    public bool CanTransitionTo(AuditDetails proposed)
    {
        if (!proposed.IsValid())
        {
            return false;
        }

        if (CreatedDate != proposed.CreatedDate || CreatedBy != proposed.CreatedBy)
        {
            return false;
        }

        if (proposed.LastModifiedDate != DateTime.MinValue && proposed.LastModifiedBy is not { Length: > 0 })
        {
            return false;
        }

        if (proposed.LastModifiedDate < LastModifiedDate || proposed.LastModifiedBy?.Length > MaxLastModifiedByLength)
        {
            return false;
        }

        return true;
    }
}
