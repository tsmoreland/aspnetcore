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

using CarvedRock.Domain.ValueObjects;

namespace CarvedRock.Domain.Entities;

public sealed class CannedFoodItem : FoodItem
{
    /// <inheritdoc />
    public CannedFoodItem(string description, decimal price, float weight, DateTime expiryDate, CanningMaterial canningMaterial, DateTime productionDate)
        : base(description, price, weight, expiryDate)
    {
        CanningMaterial = canningMaterial;
        ProductionDate = productionDate;
    }

    /// <inheritdoc />
    public CannedFoodItem(string description, decimal price, float weight, ICollection<ItemOrder> orders, DateTime expiryDate, CanningMaterial canningMaterial, DateTime productionDate)
        : base(description, price, weight, orders, expiryDate)
    {
        CanningMaterial = canningMaterial;
        ProductionDate = productionDate;
    }

    /// <inheritdoc />
    public CannedFoodItem(int id, string description, decimal price, float weight, ICollection<ItemOrder> itemOrders, DateTime expiryDate, CanningMaterial canningMaterial, DateTime productionDate)
        : base(id, description, price, weight, itemOrders, expiryDate)
    {
        CanningMaterial = canningMaterial;
        ProductionDate = productionDate;
    }

    public required CanningMaterial CanningMaterial { get; init; }
    public required DateTime ProductionDate { get; init; }
}
