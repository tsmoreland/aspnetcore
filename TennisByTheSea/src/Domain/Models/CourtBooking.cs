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

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace TennisByTheSea.Domain.Models;

public sealed class CourtBooking
{
    private int _memberId;
    private int _courtId;
    private const int NewCourtBookingId = 0;

    public static CourtBooking BookCourtForMember(Court court, Member member, DateTime startTime, TimeSpan length)
    {
        return new CourtBooking(
            NewCourtBookingId,
            court,
            member,
            startTime,
            startTime.Add(length));

    }

    [SetsRequiredMembers]
    private CourtBooking(int id, Court court, Member member, DateTime startDateTime, DateTime endDateTime)
    {
        _memberId = member.Id;
        _courtId = court.Id;

        Id = id;
        Member = member;
        Court = court;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
    }

    public int Id { get; }
    public required Member Member { get; init; }
    public required Court Court { get; init; }
    public DateTime StartDateTime { get; private set; }
    public DateTime EndDateTime { get; private set; }


#pragma warning disable IDE0051 // Remove unused private members
    /// <summary>
    /// used by EF Core and as such cannot set Court, Member or their IDs - they will be set more directly
    /// </summary>
    [SetsRequiredMembers]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private CourtBooking(int id, DateTime startDateTime, DateTime endDateTime)
    {
        _memberId = 0;
        _courtId = 0;
        Id = id;
        Member = null!;
        Court = null!;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
    }
#pragma warning restore IDE0051 // Remove unused private members
}
