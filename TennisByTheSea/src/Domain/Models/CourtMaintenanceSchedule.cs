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
using System.Diagnostics.CodeAnalysis;

namespace TennisByTheSea.Domain.Models;

public class CourtMaintenanceSchedule
{
    private readonly int _courtId;
    private const int NewCourtMaintenanceScheduleId = 0;

    public static CourtMaintenanceSchedule CreateForCourt(Court court, string workTitle, DateTime startDate, DateTime endDate,
        bool courtIsClosed)
    {
        CourtMaintenanceSchedule schedule = new(
            NewCourtMaintenanceScheduleId,
            workTitle,
            courtIsClosed,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1),
            court.Id,
            court);

        // validate dates
        schedule.SetSchedule(startDate, endDate);

        return schedule;
    }


    [SetsRequiredMembers]
    private CourtMaintenanceSchedule(int id, string workTitle, bool courtIsClosed, DateTime startDate, DateTime endDate, int courtId, Court court)
    {
        Id = id;
        WorkTitle = workTitle;
        CourtIsClosed = courtIsClosed;
        StartDate = startDate;
        EndDate = endDate;
        _courtId = courtId;
        Court = court;
    }

    public int Id { get; }
    public string WorkTitle { get; private set; }
    public bool CourtIsClosed { get; set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public required Court Court { get; init; }

    public void SetSchedule(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
        {
            throw new ArgumentException("end time cannot be before start time");
        }

        StartDate = startDate;
        EndDate = endDate;
    }

    /// <summary>
    /// used by EF Core and as such cannot set Court, Member or their IDs - they will be set more directly
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [SetsRequiredMembers]
    private CourtMaintenanceSchedule(int id, string workTitle, bool courtIsClosed, DateTime startDate, DateTime endDate, int courtId)
    {
        Id = id;
        WorkTitle = workTitle;
        CourtIsClosed = courtIsClosed;
        StartDate = startDate;
        EndDate = endDate;
        _courtId = 0;
        Court = null!;
    }

}
