using BethanysPieShopHRM.App;
using BethanysPieShopHRM.Web.App.Infrastructure;
using BethanysPieShopHRM.Web.App.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<IEmployeeDataService, EmployeeDataService>(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddHttpClient<ICountryDataService, CountryDataService>(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddHttpClient<IJobCategoryDataService, JobCategoryDataService>(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddScoped<ApplicationState>();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
