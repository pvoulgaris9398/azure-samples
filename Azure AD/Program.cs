


using Microsoft.Identity.Client;

DotNetEnv.Env.Load();

var tenantId = Environment.GetEnvironmentVariable("TENANT_ID");
var clientId = Environment.GetEnvironmentVariable("CLIENT_ID");


var app = PublicClientApplicationBuilder
    .Create(clientId)
    .WithAuthority(AzureCloudInstance.AzurePublic, tenantId)
    .WithRedirectUri("http://localhost")
    .Build(); 
string[] scopes = { "user.read" };
AuthenticationResult result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();

WriteLine($"Claims Principal:\t{result.ClaimsPrincipal}\n");
WriteLine($"Expires On:\t{result.ExpiresOn}\n");
WriteLine($"Access Token:\t{result.AccessToken}\n");
