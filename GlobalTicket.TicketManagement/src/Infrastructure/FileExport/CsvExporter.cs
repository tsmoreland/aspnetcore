//
// Copyright © 2022 Terry Moreland
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

using System.Globalization;
using CsvHelper;
using GlobalTicket.TicketManagement.Application.Contracts.Infrastructure;
using GlobalTicket.TicketManagement.Application.Features.Events.Queries.GetEventsExport;

namespace GlobalTicket.TicketManagement.Infrastructure.FileExport;

public sealed class CsvExporter : ICsvExporter
{
    /// <inheritdoc />
    public byte[] ExportEventsToCsv(IEnumerable<EventExportDto> eventExportDts)
    {
        using MemoryStream stream = new();
        using (StreamWriter streamWriter = new(stream))
        {
            using CsvWriter csvWriter = new(streamWriter, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(eventExportDts);
        }

        return stream.ToArray();
    }

    /// <inheritdoc />
    public async ValueTask<byte[]> ExportEventsToCsv(IAsyncEnumerable<EventExportDto> eventExportDtos, CancellationToken cancellationToken)
    {
        using MemoryStream stream = new();
        await using (StreamWriter streamWriter = new(stream))
        {
            await using CsvWriter csvWriter = new(streamWriter, CultureInfo.InvariantCulture);
            await foreach (EventExportDto eventExportDto in eventExportDtos.WithCancellation(cancellationToken))
            {
                csvWriter.WriteRecord(eventExportDto);
            }
        }
        return stream.ToArray();
    }
}
