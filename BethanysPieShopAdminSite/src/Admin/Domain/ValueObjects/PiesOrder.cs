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

namespace BethanysPieShop.Admin.Domain.ValueObjects;

public enum PiesOrder
{
    Id,
    Name,
    ShortDescription,
    LongDescription,
    Price,
    CategoryName,
}

public static class PiesOrderFactory
{
    public static PiesOrder FromString(string value)
    {
        return (value?.ToUpperInvariant() ?? string.Empty) switch
        {
            "NAME" => PiesOrder.Name,
            "LONGDESCRIPTION" => PiesOrder.LongDescription,
            "SHORTDESCRIPTION" => PiesOrder.ShortDescription,
            "CATEGORY" => PiesOrder.CategoryName,
            "CATEGORYNAME" => PiesOrder.CategoryName,
            _ => PiesOrder.Id,
        };
    }
}
