
using Azure;
using Azure.Data.Tables;

public class TableTest
{
    private string TableName => "Customers";
    private string ConnectionString
    {
        get
        {
            var connectionString = Environment.GetEnvironmentVariable("AZURE_COSMOSDB_CONNECTION_STRING");
            if (connectionString != null)
            {
                return connectionString;
            }
            throw new ArgumentException(nameof(ConnectionString), "Unable to find setting 'AZURE_COSMOSDB_CONNECTION_STRING'!");
        }
    }
    public async Task Run()
    {
        WriteLine("Creating Table: {0}", TableName);

        var client = await CreateTable();

        WriteLine("Calling Function: {0}", nameof(AddEntity));
        var result = await AddEntity(client, new Person { FirstName = "Joe", LastName = "Sneakers" });

        WriteLine("{0} Returned\n{1}", nameof(AddEntity), result);

    }

    private async Task<Response> AddEntity(TableClient client, ITableEntity entity)
    {
        return await client.AddEntityAsync(entity);
    }
    private async Task<TableClient> CreateTable()
    {

        var client = new TableClient(ConnectionString, TableName);
        await client.CreateIfNotExistsAsync();
        return client;
    }

}
