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

namespace TennisByTheSea.Shared;

public static class OptionalResult
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static OptionalResult<T> Of<T>(in T value)
    {
        return OptionalResult<T>.Of(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static OptionalResult<T> Empty<T>(params string[] errors)
    {
        return OptionalResult<T>.Empty(errors);
    }
}

public readonly record struct OptionalResult<T> : IEquatable<OptionalResult<T>>
{
    public OptionalResult()
        : this(default!, false, Array.Empty<string>())
    {
    }

    public static OptionalResult<T> Of(in T value)
    {
        return new OptionalResult<T>(value, true, Array.Empty<string>());
    }

    public static OptionalResult<T> Empty(params string[] errors)
    {
        return new OptionalResult<T>(default!, false, errors);
    }

    private OptionalResult(T value, bool hasValue, IEnumerable<string> errors)
    {
        Value = value;
        HasValue = hasValue;
        Errors = errors;
    }

    public T Value { get; } = default!;
    public bool HasValue { get; }
    public IEnumerable<string> Errors { get; }
}
