using System.Net.Http.Headers;
using AutoMapper;
using Blazored.LocalStorage;
using GloboTicket.TicketManagement.UI.ApiClient.Services;

namespace GloboTicket.TicketManagement.UI.BlazorWasm.App.Services;

public abstract class BaseDataService
{
    protected IClient Client { get; }
    protected IMapper Mapper { get; }
    protected ILocalStorageService LocalStorageService { get; }

    protected internal BaseDataService(IClient client, IMapper mapper, ILocalStorageService localStorageService)
    {
        Client = client ?? throw new ArgumentNullException(nameof(client));
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        LocalStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));
    }

    protected static ApiResponse<T> ConvertApiException<T>(ApiException ex)
    {
        return ex.StatusCode switch
        {
            400 => ApiResponse.Error<T>("Validation errors have occured.", ex.Response),
            404 => ApiResponse.Error<T>("The requested item could not be found.", string.Empty),
            _ => ApiResponse.Error<T>("Something went wrong, please try again.", string.Empty),
        };
    }

    protected static ApiResponse<T> ConvertException<T>(Exception ex)
    {
        return ApiResponse.Error<T>("Unexpected error occurred.", ex.Message);
    }

    protected async Task AddBearerToken()
    {
        if (await LocalStorageService.ContainKeyAsync("token"))
        {
            Client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await LocalStorageService.GetItemAsync<string>("token"));
        }
    }
}
