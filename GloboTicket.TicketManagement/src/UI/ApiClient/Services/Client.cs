namespace GloboTicket.TicketManagement.UI.ApiClient.Services;

public partial class Client : IClient
{
    /// <inheritdoc />
    // ReSharper disable once ConvertToAutoProperty -- generated code, converting to auto property which mean manual updates after re-generation
    public HttpClient HttpClient => _httpClient;

}
