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

using System.Runtime.CompilerServices;

namespace GloboTicket.Shop.Shared.Validators;

public static class ArgumentValidator
{

    public static string RequiresNonEmptyStringWithMaxLength(string argument, int maxLength,
        [CallerArgumentExpression("argument")] string? parameterName = null, string? message = null)
    {
        if (argument is not { Length: > 0 } || argument.Length > maxLength)
        {
            throw new ArgumentException(parameterName, message);
        }

        return argument;
    }

    public static string? OptionalStringWithMaxLength(string? argument, int maxLength,
        [CallerArgumentExpression("argument")] string? parameterName = null, string? message = null)
    {
        if (argument is null || argument.Length <= maxLength)
        {
            return argument;
        }

        throw new ArgumentException(parameterName, message);
    }

    public static int RequiresGreaterThanOrEqualToZero(int argument, [CallerArgumentExpression("argument")] string? parameterName = null, string? message = null)
    {
        if (argument < 0)
        {
            throw new ArgumentOutOfRangeException(parameterName, message);
        }
        return argument;
    }

    public static DateTime RequiresNowOrNewer(DateTime argument, [CallerArgumentExpression("argument")] string? parameterName = null, string? message = null)
    {
        if (argument < DateTime.UtcNow)
        {
            throw new ArgumentException(parameterName, message);
        }

        return argument;
    }

    public static IReadOnlyList<T> RequiresNonEmptyCollection<T>(IReadOnlyList<T> argument,
        [CallerArgumentExpression("argument")] string? parameterName = null, string? message = null)
    {
        if (argument is not { Count: > 0 })
        {
            throw new ArgumentException(parameterName, message);
        }

        return argument;
    }
}
