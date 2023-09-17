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

using BethanysPieShop.Admin.Domain.Models;
using BethanysPieShop.Admin.Domain.ValueObjects;

namespace BethanysPieShop.Admin.Infrastructure.Persistence.Extensions;

internal static class QueryableExtensions
{
    public static IOrderedQueryable<Category> OrderBy(this IQueryable<Category> source, CategoriesOrder order, bool descending) =>
        descending
        ? source.OrderByDescending(order)
        : source.OrderBy(order);

    public static IOrderedQueryable<Category> OrderBy(this IQueryable<Category> source, CategoriesOrder order)
    {
        return order switch
        {
            CategoriesOrder.Name => source.OrderBy(e => e.Name),
            CategoriesOrder.Description => source.OrderBy(e => e.Description),
            CategoriesOrder.DateAdded => source.OrderBy(e => e.DateAdded),
            _ => source.OrderBy(e => e.Id),
        };
    }

    public static IOrderedQueryable<Category> OrderByDescending(this IQueryable<Category> source, CategoriesOrder order)
    {
        return order switch
        {
            CategoriesOrder.Name => source.OrderByDescending(e => e.Name),
            CategoriesOrder.Description => source.OrderByDescending(e => e.Description),
            CategoriesOrder.DateAdded => source.OrderByDescending(e => e.DateAdded),
            _ => source.OrderByDescending(e => e.Id),
        };
    }

    public static IOrderedQueryable<Pie> OrderBy(this IQueryable<Pie> source, PiesOrder order, bool descending) =>
        descending
        ? source.OrderByDescending(order)
        : source.OrderBy(order);

    public static IOrderedQueryable<Pie> OrderBy(this IQueryable<Pie> source, PiesOrder order)
    {
        return order switch
        {
            PiesOrder.Name => source.OrderBy(e => e.Name),
            PiesOrder.ShortDescription => source.OrderBy(e => e.ShortDescription),
            PiesOrder.LongDescription => source.OrderBy(e => e.LongDescription),
            PiesOrder.Price => source.OrderBy(e => e.Price),
            PiesOrder.CategoryName => source.OrderBy(e => e.CategoryName),
            _ => source.OrderBy(e => e.Id),
        };
    }

    public static IOrderedQueryable<Pie> OrderByDescending(this IQueryable<Pie> source, PiesOrder order)
    {
        return order switch
        {
            PiesOrder.Name => source.OrderByDescending(e => e.Name),
            PiesOrder.ShortDescription => source.OrderByDescending(e => e.ShortDescription),
            PiesOrder.LongDescription => source.OrderByDescending(e => e.LongDescription),
            PiesOrder.Price => source.OrderByDescending(e => e.Price),
            PiesOrder.CategoryName => source.OrderByDescending(e => e.CategoryName),
            _ => source.OrderByDescending(e => e.Id),
        };
    }

    public static IOrderedQueryable<Order> OrderBy(this IQueryable<Order> source, OrdersOrder order, bool descending) =>
        descending
        ? source.OrderByDescending(order)
        : source.OrderBy(order);

    public static IOrderedQueryable<Order> OrderBy(this IQueryable<Order> source, OrdersOrder order)
    {
        return order switch
        {
            OrdersOrder.FirstName => source.OrderBy(e => e.FirstName),
            OrdersOrder.LastName => source.OrderBy(e => e.LastName),
            OrdersOrder.AddresssLine1 => source.OrderBy(e => e.AddressLine1),
            OrdersOrder.City => source.OrderBy(e => e.City),
            _ => source.OrderBy(e => e.Id),
        };
    }

    public static IOrderedQueryable<Order> OrderByDescending(this IQueryable<Order> source, OrdersOrder order)
    {
        return order switch
        {
            OrdersOrder.FirstName => source.OrderByDescending(e => e.FirstName),
            OrdersOrder.LastName => source.OrderByDescending(e => e.LastName),
            OrdersOrder.AddresssLine1 => source.OrderByDescending(e => e.AddressLine1),
            OrdersOrder.City => source.OrderBy(e => e.City),
            _ => source.OrderByDescending(e => e.Id),
        };
    }
}
