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

namespace TennisByTheSea.Domain.Models;

public sealed class Member
{
    private readonly HashSet<CourtBooking> _courtBookings;
    private readonly string _userId;
    private const int NewMemberId = 0;

    public static Member CreateNewMember(string forename, string surname, string userId)
    {
        DateTime now = DateTime.UtcNow;
        DateOnly joinDate = new(now.Year, now.Month, now.Day);
        return new Member(NewMemberId, forename, surname, joinDate, new HashSet<CourtBooking>(), userId);
    }

    private Member(int id, string forename, string surname, DateOnly joinDate, HashSet<CourtBooking> courtBookings, string userId)
    {
        _userId = userId;
        Id = id;
        Forename = forename;
        Surname = surname;
        JoinDate = joinDate;
        _courtBookings = courtBookings;
    }


    public int Id { get; }
    public string Forename { get; set; }
    public string Surname { get; set; }
    public DateOnly JoinDate { get; set; }

    public IEnumerable<CourtBooking> CourtBookings => _courtBookings.ToList();
}
