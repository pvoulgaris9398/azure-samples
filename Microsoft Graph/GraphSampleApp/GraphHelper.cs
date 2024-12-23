
using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Me.SendMail;

public class GraphHelper{

private static DeviceCodeCredential? _deviceCodeCredential;

private static GraphServiceClient? _userClient;

private static string[] _scopes = new [] {"user.read"};

public static void InitializeGraphForUserAuth(
    string tenantId,
    string clientId,
    string[] scopes,
    Func<DeviceCodeInfo, CancellationToken, Task> deviceCodePrompt
)
{
    _scopes = scopes;

    var options = new DeviceCodeCredentialOptions
    {
        ClientId = clientId,
        TenantId = tenantId,
        DeviceCodeCallback = deviceCodePrompt
    };

    _deviceCodeCredential = new DeviceCodeCredential(options);

    _userClient = new GraphServiceClient(_deviceCodeCredential, scopes);

}

public static async Task<string> GetUserTokenAsync()
{
    _ = _deviceCodeCredential ?? throw new System.NullReferenceException("Graph has not been initialized for user auth");

    var context = new TokenRequestContext(_scopes);
    var response = await _deviceCodeCredential.GetTokenAsync(context);
    return response.Token;

}
}