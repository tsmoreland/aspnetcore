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

using System.Diagnostics.CodeAnalysis;

namespace TennisByTheSea.Shared.Exceptions;

public sealed class InvalidConfigurationException : Exception
{
    /// <inheritdoc />
    public InvalidConfigurationException()
        : this(null)
    {
    }

    /// <inheritdoc />
    public InvalidConfigurationException(string? message) 
        : this(message, null)
    {
    }

    /// <inheritdoc />
    public InvalidConfigurationException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public static void ThrowIf([DoesNotReturnIf(true)] bool condition, string? message)
    {
        if (condition)
        {
            throw new InvalidConfigurationException(message);
        }
    }
}
