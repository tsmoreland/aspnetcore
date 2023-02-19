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

public sealed class CustomerDetails
{
    public static readonly int MaxNameLength = 200;
    public static readonly int MaxEmailLength = 200;

    private CustomerDetails()
    {
        // For EF/Serialization 
        BillingAddress = Address.Invalid;
        DeliveryAddress  = Address.Invalid;
    }

    public CustomerDetails(string name, string email, Address deliveryAddress, Address billingAddress)
    {
        Name = RequiresNonEmptyStringWithMaxLength(name, MaxNameLength);
        Email = RequiresNonEmptyStringWithMaxLength(email, MaxEmailLength);
        DeliveryAddress = deliveryAddress ?? throw new ArgumentNullException(nameof(deliveryAddress));
        BillingAddress = billingAddress ?? throw new ArgumentNullException(nameof(billingAddress));

    }

    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;

    public Address BillingAddress { get; set; } 
    public Address DeliveryAddress { get; set; }

    // TODO:
    // Add Payment Id - reference to an external entity of some sort something with extra protections
    // may also want an id back to the a separate customer entity, these details are only for a given transaction the addresses
    // at the time of purchase
}
