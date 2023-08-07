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

namespace GloboTicket.Shop.Ordering.Domain.Models;

public sealed record class Address(string AddressLineOne, string? AddressLineTwo, string PostCode, string City, string Country)
{
    public static readonly int MaxAddressLineLength = 250;
    public static readonly int MaxCityLength = 250;
    public static readonly int MaxPostCodeLength = 10;
    public static readonly int MaxCountryLength = 250;

    internal static Address Invalid { get; } = new("invalid", null, "invalid", "invalid", "invalid");

    public string AddressLineOne { get; init; } = RequiresNonEmptyStringWithMaxLength(AddressLineOne, MaxAddressLineLength);
    public string? AddressLineTwo { get; init; } = OptionalStringWithMaxLength(AddressLineTwo, MaxAddressLineLength);
    public string PostCode { get; init; } = RequiresNonEmptyStringWithMaxLength(PostCode, MaxPostCodeLength);
    public string City { get; init; } = RequiresNonEmptyStringWithMaxLength(City, MaxCityLength);
    public string Country { get; init; } = RequiresNonEmptyStringWithMaxLength(Country, MaxCountryLength);
}
