using Blazored.LocalStorage;
using GloboTicket.TicketManagement.UI.ApiClient.Contracts;
using GloboTicket.TicketManagement.UI.ApiClient.Services;
using GloboTicket.TicketManagement.UI.BlazorWasm.App;
using GloboTicket.TicketManagement.UI.BlazorWasm.App.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
    .AddBlazoredLocalStorage()
    .AddAuthorizationCore();

builder.Services
    .AddSingleton(new HttpClient { BaseAddress = new Uri("https://localhost:7001") });

builder.Services
    .AddHttpClient<IClient, Client>(client => client.BaseAddress = new Uri("https://localhost:7001"));
    //.AddHttpMessageHandler<CustomAuthorizationHandler>();

builder.Services
    //.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>()
    //.AddScoped<IAuthenticationService, AuthenticationService>()
    .AddScoped<IEventDataService, EventDataService>()
    .AddScoped<ICategoryDataService, CategoryDataService>()
    .AddScoped<IOrderDataService, OrderDataService>();

await builder.Build().RunAsync();
