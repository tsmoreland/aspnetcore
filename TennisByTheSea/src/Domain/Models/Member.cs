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

using System.ComponentModel;
using TennisByTheSea.Domain.Extensions;

namespace TennisByTheSea.Domain.Models;

public sealed class Member
{
    private readonly HashSet<CourtBooking> _courtBookings ;
    private string _forename;
    private string _surname;
    private readonly DateTime _joinDate;
    private const int NewMemberId = 0;

    public static Member CreateNewMember(string forename, string surname, string userId)
    {
        DateTime now = DateTime.UtcNow;
        return new Member(NewMemberId, forename, surname, now, userId);
    }

    private Member(int id, string forename, string surname, DateTime joinDate, string userId)
    {
        Id = id;
        _forename = forename;
        _surname = surname;
        _joinDate = joinDate;
        UserId = userId;
        _courtBookings = new HashSet<CourtBooking>();
    }

    public int Id { get; }
    public string Forename
    {
        get => _forename;
        set
        {
            if (value is not { Length: > 0 })
            {
                throw new ArgumentException("cannot be null or empty");
            }

            _forename = value;
        }
    }
    public string Surname
    {
        get => _surname;
        set
        {
            if (value is not { Length: > 0 })
            {
                throw new ArgumentException("cannot be null or empty");
            }

            _surname = value;
        }
    }

    public DateOnly JoinDate => _joinDate.ToDateOnly();

    public string UserId { get; init; }

    public IEnumerable<CourtBooking> CourtBookings => _courtBookings.ToList();

    public void AddBooking(CourtBooking booking)
    {
        _courtBookings.Add(booking);
    }
}
