namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsExport;

public sealed record class EventExportFileViewModel(string EventExportFilename, string ContentType, byte[] Data);
