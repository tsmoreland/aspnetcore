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

using System.Diagnostics.CodeAnalysis;
using GloboTicket.Shop.Catalog.Domain.Models;

namespace GloboTicket.Shop.Catalog.Application.Features.Concerts.Queries.GetConcerts;

public sealed class ConcertDto
{
    public ConcertDto()
    {
    }

    [SetsRequiredMembers]
    public ConcertDto(Concert concert)
    {
        ArgumentNullException.ThrowIfNull(concert);

        Id = concert.ConcertId;
        Name = concert.Name;
        Artist = concert.Artist;
        Price = concert.Price;
        Date = concert.Date;
        Description = concert.Description;
        ImageUrl = concert.ImageUrl;
    }

    [SetsRequiredMembers]
    public ConcertDto(Guid id, string name, string artist, DateTime date, int price, string description, string? imageUrl)
    {
        Id = id;
        Name = name;
        Artist = artist;
        Date = date;
        Price = price;
        Description = description;
        ImageUrl = imageUrl;
    }


    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string Artist { get; init; }

    public required DateTime Date { get; init; }

    public required int Price { get; init; }

    public string Description { get; init; } = string.Empty;
    public string? ImageUrl { get; init; } 

}
