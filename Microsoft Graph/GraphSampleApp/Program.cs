using Microsoft.Graph;

WriteLine(".NET Graph Tutorial\n");

DotNetEnv.Env.Load();

var tenantId = Environment.GetEnvironmentVariable("TENANT_ID")??"Test";
var clientId =  Environment.GetEnvironmentVariable("CLIENT_ID")??"Test";
var scopes = new [] {"user.read"};

WriteLine(tenantId);
WriteLine(clientId);
WriteLine(scopes);

// Initialize Graph
InitializeGraph(tenantId, clientId, scopes);

await DisplayAccessTokenAsync();

// Greet the user by name
await GreetUserAsync();

void InitializeGraph(string tenantId, string clientId, string[] scopes)
{
    GraphHelper.InitializeGraphForUserAuth(
        tenantId,
        clientId,
        scopes,
        (info, cancel) => {
            Console.WriteLine(info.Message);
            return Task.FromResult(0);
        }
    );
}

async Task DisplayAccessTokenAsync()
{
    try
    {
        var userToken = await GraphHelper.GetUserTokenAsync();
        Console.WriteLine($"User token: {userToken}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error getting user access token: {ex.Message}");
    }
}

Task GreetUserAsync()
{
    return Task.FromResult(0);
}

