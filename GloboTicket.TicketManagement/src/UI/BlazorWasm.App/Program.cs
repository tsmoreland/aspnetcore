using Blazored.LocalStorage;
using GloboTicket.TicketManagement.UI.ApiClient.Contracts;
using GloboTicket.TicketManagement.UI.ApiClient.Services;
using GloboTicket.TicketManagement.UI.BlazorWasm.App;
using GloboTicket.TicketManagement.UI.BlazorWasm.App.Auth;
using GloboTicket.TicketManagement.UI.BlazorWasm.App.Contracts;
using GloboTicket.TicketManagement.UI.BlazorWasm.App.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
    .AddBlazoredLocalStorage()
    .AddAuthorizationCore();

builder.Services
    .AddSingleton(new HttpClient { BaseAddress = new Uri("https://localhost:7001") });

builder.Services
    .AddTransient<BearerTokenInLocalStorageAuthorizationMessageHandler>()
    .AddHttpClient<IClient, Client>(client => client.BaseAddress = new Uri("https://localhost:7001"))
    .AddHttpMessageHandler<BearerTokenInLocalStorageAuthorizationMessageHandler>();

builder.Services
    .AddScoped<AuthenticationStateProvider, BearerTokenInLocalStorageAuthenticationStateProvider>()
    .AddScoped<IAuthenticationService, AuthenticationService>()
    .AddScoped<IEventDataService, EventDataService>()
    .AddScoped<ICategoryDataService, CategoryDataService>()
    .AddScoped<IOrderDataService, OrderDataService>()
    .AddScoped<ITokenRepository, TokenRepository>();

await builder.Build().RunAsync();
