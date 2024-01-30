using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddUserSecrets(typeof(Program).Assembly)
    .Build();

string clientId = configuration["AzureAd:ClientId"] ?? string.Empty;
string clientSecret = configuration["AzureAd:ClientSecret"] ?? string.Empty;
string tenantId = configuration["AzureAd:TenantId"] ?? string.Empty;
string[] scopes = [configuration["AzureAd:Scope"] ?? string.Empty];
string instance = configuration["AzureAd:Instance"] ?? string.Empty;

ConfidentialClientApplicationOptions options = new()
{
    ClientId = clientId,
    TenantId = tenantId,
    ClientSecret = clientSecret,
    Instance = instance
};


IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
    .CreateWithApplicationOptions(options)
    .Build();

try
{
    AuthenticationResult result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
    Console.WriteLine("Authentication success");
    Console.WriteLine(result.AccessToken);
    Console.WriteLine($"Expires on {result.ExpiresOn}");

}
catch (Exception ex)
{
    Console.WriteLine("Authentication failed");
    Console.WriteLine(ex);
}

