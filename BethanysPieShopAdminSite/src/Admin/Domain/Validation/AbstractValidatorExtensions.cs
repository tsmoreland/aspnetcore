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

using FluentValidation;

namespace BethanysPieShop.Admin.Domain.Validation;

public static class AbstractValidatorExtensions
{
    /// <summary>
    /// validates <paramref name="value"/> using <paramref name="validator"/> throwing an exception if invalid;
    /// otherwise returning <paramref name="value"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="validator">The validator used to verify <paramref name="value"/></param>
    /// <param name="value">the value to be validated</param>
    /// <returns>the value if valid</returns>
    /// <exception cref="FluentValidation.ValidationException">if <paramref name="value"/> is invalid</exception>
    public static T GetValidatedValueOrThrow<T>(this AbstractValidator<T> validator, T value)
    {
        validator.ValidateAndThrow(value);
        return value;
    }
}
