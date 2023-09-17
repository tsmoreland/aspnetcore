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

namespace BethanysPieShop.Admin.Domain.Validation;

internal sealed class AllergyInformationValidator : NullableStringValidator
{
    private static readonly Lazy<AllergyInformationValidator> s_instance = new(() => new AllergyInformationValidator());

    /// <inheritdoc />
    public AllergyInformationValidator()
    {
        Initialize();
    }

    public static AllergyInformationValidator Instance => s_instance.Value;

    /// <inheritdoc />
    protected override int MinimumLength => 0;

    /// <inheritdoc />
    protected override int MaximumLength => 1000;

    /// <inheritdoc />
    protected override bool AllowNull => true;
}
