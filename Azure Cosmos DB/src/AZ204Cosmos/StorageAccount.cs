using Azure;
using Azure.Data.Tables;

namespace AZ204Cosmos
{
    public class StorageAccount
    {
        private static string TableName
        {
            get
            {
                var table = Environment.GetEnvironmentVariable("STORAGE_ACCOUNT_TABLE_NAME");
                return table ?? throw new ArgumentException(nameof(table), "Unable to find setting 'STORAGE_ACCOUNT_TABLE_NAME'!");
            }
        }
        private string ConnectionString
        {
            get
            {
                var connectionString = Environment.GetEnvironmentVariable("AZURE_TABLE_CONNECTION_STRING");
                return connectionString ?? throw new ArgumentException(nameof(ConnectionString), "Unable to find setting 'AZURE_TABLE_CONNECTION_STRING'!");
            }
        }
        public async Task Run()
        {
            WriteLine("Creating Table: {0}", TableName);
            var client = await CreateTable();
            while (true)
            {
                WriteLine("Would you like to add another record? (Y/N) [Default:N]");
                if (ReadKey(true).Key == ConsoleKey.Y)
                {
                    WriteLine("Calling Function: {0}", nameof(AddEntity));
                    var result = await AddEntity(client, new Person { FirstName = RandomElementFrom(FirstNames), LastName = RandomElementFrom(LastNames) });

                    WriteLine("{0} Returned\n{1}", nameof(AddEntity), result);
                }
                else
                {
                    WriteLine("Would you like to run the query? (Y/N) [Default:N]");
                    if (ReadKey(true).Key == ConsoleKey.Y)
                    {
                        Query(client);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private static void Query(TableClient client)
        {
            var results = client.Query<TableEntity>();
            WriteLine($"The query returned {results.Count()} entities.");
            foreach (var entity in results)
            {
                WriteLine($"{entity.GetString("RowKey")}: {entity.GetString("LastName")}, {entity.GetString("FirstName")}");
            }

        }

        private static async Task<Response> AddEntity(TableClient client, ITableEntity entity) => await client.AddEntityAsync(entity);
        private async Task<TableClient> CreateTable()
        {

            var client = new TableClient(ConnectionString, TableName);
            _ = await client.CreateIfNotExistsAsync();
            return client;
        }

        private static string RandomElementFrom(string[] elements) => elements[new Random().Next(0, elements.Length)];

        private static readonly string[] FirstNames = ["Adam", "Bobby", "Chuck", "Dave", "Eddie", "Frank", "George", "Harry", "Ichabod", "Jack", "Kyle", "Leo", "Mike", "Nathan"];

        private static readonly string[] LastNames = ["Ablex", "Bali", "Chinook", "Davros", "Ezra", "Fusilade", "Gregorious", "Hanky", "Iapetus", "Joliet", "King", "Lazarus", "Morpheus", "Nemesis"];

    }
}