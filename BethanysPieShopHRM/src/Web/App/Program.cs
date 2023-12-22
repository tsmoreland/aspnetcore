using BethanysPieShopHRM.Web.App;
using BethanysPieShopHRM.Web.App.Extensions;
using BethanysPieShopHRM.Web.App.Infrastructure;
using BethanysPieShopHRM.Web.App.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder
    .AddHttpClientToServices<IEmployeeDataService, EmployeeDataService>()
    .AddHttpClientToServices<ICountryDataService, CountryDataService>()
    .AddHttpClientToServices<IJobCategoryDataService, JobCategoryDataService>();

builder.Services
    .AddScoped<ApplicationState>()
    .AddBlazoredLocalStorage()
    .AddOidcAuthentication(options =>
    {
        builder.Configuration.Bind("Auth0", options.ProviderOptions);
        options.ProviderOptions.ResponseType = "code";
        options.ProviderOptions.AdditionalProviderParameters
            .Add("audience", builder.Configuration["Auth0:Audience"] ?? string.Empty);

    });

await builder.Build().RunAsync();

