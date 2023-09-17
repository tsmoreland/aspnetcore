namespace GloboTicket.TicketManagement.UI.ApiClient.Services;

public static class ApiResponse
{
    public static ApiResponse<T> Success<T>(T data)
    {
        return new ApiResponse<T>(string.Empty, null, true, data);
    }

    public static ApiResponse<T> Error<T>(string message, string? problem)
    {
        return new ApiResponse<T>(message, problem is not null ?  new Dictionary<string, string>() { ["validation"] = problem } : null, false, default!);
    }
    public static ApiResponse<T> FromValidationErrors<T>(string message, IDictionary<string, string>? problems)
    {
        return new ApiResponse<T>(message, problems?.AsReadOnly(), false, default!);
    }
}

public sealed record class ApiResponse<T>(string Message, IReadOnlyDictionary<string, string>? ProblemDetails, bool Success, T Data);
