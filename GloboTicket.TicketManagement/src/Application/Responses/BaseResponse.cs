namespace GloboTicket.TicketManagement.Application.Responses;

public abstract record class BaseResponse(bool Success, string? Message, IReadOnlyDictionary<string, string>? ValidationErrors)
{
    protected BaseResponse(bool success)
        : this(success, null, null)
    {
    }

    protected BaseResponse()
        : this(true, null, null)
    {
    }
}
