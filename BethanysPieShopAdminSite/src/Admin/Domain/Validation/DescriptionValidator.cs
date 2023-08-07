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

using FluentValidation;

namespace BethanysPieShop.Admin.Domain.Validation;

internal sealed class DescriptionValidator : AbstractValidator<string?>
{
    private static readonly Lazy<DescriptionValidator> s_instance = new(() => new DescriptionValidator());

    /// <inheritdoc />
    public DescriptionValidator()
    {
        RuleFor<string>(vvalue => value)
            .Must(value => value is null || !string.IsNullOrWhiteSpace(value))
            .Must(value => value is null || value.Length is >= 5 and <= 500)
            .WithMessage("Description must eithe be null or have a length between 5 and 500");
    }

    public static DescriptionValidator Instance => s_instance.Value;
}
