﻿using Microsoft.Azure.Cosmos;

public class Program
{
    // Replace <documentEndpoint> with the information created earlier
    private static readonly string EndpointUri = "https://pvoulgaris9398.documents.azure.com:443/";

    // Set variable to the Primary Key from earlier.
	// No worries, this key is no longer active, nor would I make it public in source code if it was.
	// This should come from an environment variable or an Azure Key Vault secret.
    private static readonly string PrimaryKey = "HHKR2BlFThNjJQuR3Cqk9uzAXAUJDpGqtsqxFz7TauQpCg3VEUKU5XiPymyxHKEYsVIMaC2BQn5jACDbWQZlpg==";

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
            Program p = new Program();
            await p.CosmosAsync();

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
    this.database = await this.cosmosClient?.CreateDatabaseIfNotExistsAsync(databaseId);
    Console.WriteLine("Created Database: {0}\n", this.database.Id);
}

private async Task CreateContainerAsync()
{
    // Create a new container
    this.container = await this.database?.CreateContainerIfNotExistsAsync(containerId, "/LastName");
    Console.WriteLine("Created Container: {0}\n", this.container.Id);
}


}