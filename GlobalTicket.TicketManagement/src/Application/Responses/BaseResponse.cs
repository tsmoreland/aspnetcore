namespace GlobalTicket.TicketManagement.Application.Responses;

public abstract record class BaseResponse(bool Success, string? Message, IReadOnlyList<string>? ValidationErrors)
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
