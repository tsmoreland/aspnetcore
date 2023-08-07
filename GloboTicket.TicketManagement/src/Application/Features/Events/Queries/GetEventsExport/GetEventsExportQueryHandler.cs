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

using AutoMapper;
using GloboTicket.TicketManagement.Domain.Contracts.Events.Queries;
using GloboTicket.TicketManagement.Domain.Contracts.Infrastructure;
using GloboTicket.TicketManagement.Domain.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsExport;

public sealed class GetEventsExportQueryHandler : IRequestHandler<GetEventsExportQuery, EventExportFileViewModel>
{
    private readonly IAsyncRepository<Event> _eventRepository;
    private readonly ICsvExporter _csvExporter;

    public GetEventsExportQueryHandler(IAsyncRepository<Event> eventRepository, ICsvExporter csvExporter)
    {
        _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        _csvExporter = csvExporter ?? throw new ArgumentNullException(nameof(csvExporter));
    }

    /// <inheritdoc />
    public async Task<EventExportFileViewModel> Handle(GetEventsExportQuery request, CancellationToken cancellationToken)
    {
        IAsyncEnumerable<EventExportDto> allEvents = _eventRepository.GetAll(new EventExportDtoSelectionSpecification(), cancellationToken);
        byte[] fileData = await _csvExporter.ExportEventsToCsv(allEvents, cancellationToken);

        EventExportFileViewModel eventExportFileViewModel = new($"{Guid.NewGuid()}.csv", "text/csv", fileData);
        return eventExportFileViewModel;
    }
}
