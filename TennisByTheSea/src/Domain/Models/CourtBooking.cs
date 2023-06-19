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

public sealed class CourtBooking
{
    private int _memberId;
    private int _courtId;
    private Court _court;
    private Member _member;
    private const int NewCourtBookingId = 0;

    public static CourtBooking BookCourtForMember(Court court, Member member, DateTime startTime, TimeSpan length)
    {
        return new CourtBooking(
            NewCourtBookingId,
            court.Id,
            court,
            member.Id,
            member,
            startTime,
            startTime.Add(length));

    }

    private CourtBooking(int id, int courtId, Court court, int memberId, Member member, DateTime startDateTime, DateTime endDateTime)
    {
        _memberId = memberId;
        _courtId = courtId;

        Id = id;
        _member = member;
        _court = court;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
    }


    public int Id { get; }
    public Member Member
    {
        get => _member;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _memberId = value.Id;
            _member = value;
        }
    }
    public Court Court
    {
        get => _court;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _courtId = value.Id;
            _court = value;
        }
    }
    public DateTime StartDateTime { get; private set; }
    public DateTime EndDateTime { get; private set; }
}
