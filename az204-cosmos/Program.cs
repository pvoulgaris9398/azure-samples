using dotenv.net;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure;

public class Program
{
    // Replace <documentEndpoint> with the information created earlier
    private static readonly string EndpointUri = "";
    // This should come from an environment variable or an Azure Key Vault secret.
    private static readonly string PrimaryKey = "";

    // The Cosmos client instance
    private CosmosClient? cosmosClient;

    // The database we will create
    private Database? database;

    // The container we will create.
    private Container? container;

    // The names of the database and container we will create
    private string databaseId = "az204Database";
    private string containerId = "az204Container";

    public static async Task Main(string[] args)
    {
        try
        {
            Console.WriteLine("Beginning operations...\n");
            DotEnv.Load();
            var t = new StorageAccount();
            await t.Run();

        }
        catch (CosmosException de)
        {
            Exception baseException = de.GetBaseException();
            Console.WriteLine("{0} error occurred: {1}", de.StatusCode, de);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: {0}", e);
        }
        finally
        {
            Console.WriteLine("End of program, press any key to exit.");
            Console.ReadKey();
        }
    }

    public async Task CosmosAsync()
    {
        // Create a new instance of the Cosmos Client
        this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);

        // Runs the CreateDatabaseAsync method
        await this.CreateDatabaseAsync();

        // Run the CreateContainerAsync method
        await this.CreateContainerAsync();
    }

    private async Task CreateDatabaseAsync()
    {
        // Create a new database using the cosmosClient
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        database = await cosmosClient?.CreateDatabaseIfNotExistsAsync(databaseId);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        Console.WriteLine("Created Database: {0}\n", this.database.Id);
    }

    private async Task CreateContainerAsync()
    {
        // Create a new container
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        container = await database?.CreateContainerIfNotExistsAsync(containerId, "/LastName");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        Console.WriteLine("Created Container: {0}\n", this.container.Id);
    }


}
