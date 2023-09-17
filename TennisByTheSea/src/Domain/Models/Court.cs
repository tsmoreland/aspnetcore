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

namespace TennisByTheSea.Domain.Models;

public sealed class Court
{
    private readonly HashSet<CourtBooking> _courtBookings;
    private const int NewCourtId = 0;


    public static Court AddCourt(CourtType type, string name)
    {
        return new Court(NewCourtId, type, name);
    }

    private Court(int id, CourtType type, string name)
    {
        Id = id;
        Type = type;
        Name = name;
        _courtBookings = new HashSet<CourtBooking>();
    }

    public int Id { get; set; }
    public CourtType Type { get; }
    public string Name { get; }
    public IEnumerable<CourtBooking> Bookings => _courtBookings.ToList();

    public bool TryBookCourtForMember(Court court, Member member, DateTime startTime, TimeSpan length)
    {
        CourtBooking booking = CourtBooking.BookCourtForMember(court, member, startTime, length);
        // TODO: check for conflicting time in _courtBookings
        _courtBookings.Add(booking);
        return true;
    }
}
