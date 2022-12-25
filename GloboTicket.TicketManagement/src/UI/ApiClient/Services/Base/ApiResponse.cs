namespace GloboTicket.TicketManagement.UI.ApiClient.Services.Base;

public sealed record class ApiResponse<T>(string Message, ProblemDetails? Problem, bool Success, T? Data);
