using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddUserSecrets(typeof(Program).Assembly)
    .Build();

var clientId = configuration["AzureAd:ClientId"] ?? string.Empty;
var clientSecret = configuration["AzureAd:ClientSecret"] ?? string.Empty;
var tenantId = configuration["AzureAd:TenantId"] ?? string.Empty;
string[] scopes = [configuration["AzureAd:Scope"] ?? string.Empty];
var instance = configuration["AzureAd:Instance"] ?? string.Empty;

ConfidentialClientApplicationOptions options = new()
{
    ClientId = clientId,
    TenantId = tenantId,
    ClientSecret = clientSecret,
    Instance = instance
};


var app = ConfidentialClientApplicationBuilder
    .CreateWithApplicationOptions(options)
    .Build();

try
{
    var result = await app.AcquireTokenForClient(scopes).ExecuteAsync().ConfigureAwait(false);
    Console.WriteLine("Authentication success");
    Console.WriteLine(result.AccessToken);
    Console.WriteLine($"Expires on {result.ExpiresOn}");
}
catch (Exception ex)
{
    Console.WriteLine("Authentication failed");
    Console.WriteLine(ex);
}

