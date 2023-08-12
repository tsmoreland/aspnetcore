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

internal sealed class PhoneNumberValidator : AbstractValidator<string>
{
    private static readonly Lazy<PhoneNumberValidator> s_instance = new(() => new PhoneNumberValidator());
    /// <inheritdoc />
    public PhoneNumberValidator()
    {
        // TODO: add regex to match phone number
        RuleFor<string>(value => value)
            .Must(s => !string.IsNullOrWhiteSpace(s))
            .MaximumLength(15)
            .WithMessage("Invalid Phone Number");
    }
    public static PhoneNumberValidator Instance => s_instance.Value;
}
