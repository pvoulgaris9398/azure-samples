using System.Net;
using Microsoft.Azure.Cosmos;
public class Cosmos
{
    private string EndpointUri
    {
        get
        {
            var value = Environment.GetEnvironmentVariable ("COSMOS_ENDPOINT_URI");
            if (value != null)
            {
                return value;
            }
            throw new ArgumentException(nameof(value), "Unable to find setting 'COSMOS_ENDPOINT_URI'!");
        }
    }

    private string PrimaryKey
    {
        get
        {
            var value = Environment.GetEnvironmentVariable("COSMOS_PRIMARY_KEY");
            if (value != null)
            {
                return value;
            }
            throw new ArgumentException(nameof(value), "Unable to find setting 'COSMOS_PRIMARY_KEY'!");
        }
    }

    private string DatabaseId
    {
        get
        {
            var value = Environment.GetEnvironmentVariable("COSMOS_DATABASE_NAME");
            if (value != null)
            {
                return value;
            }
            throw new ArgumentException(nameof(value), "Unable to find setting 'COSMOS_DATABASE_NAME'!");
        }
    }
    private string ContainerId
    {
        get
        {
            var value = Environment.GetEnvironmentVariable("COSMOS_CONTAINER_NAME");
            if (value != null)
            {
                return value;
            }
            throw new ArgumentException(nameof(value), "Unable to find setting 'COSMOS_CONTAINER_NAME'!");
        }
    }

    private string PartitionKeyPath
    {
        get
        {
            var value = Environment.GetEnvironmentVariable("COSMOS_PARTITION_KEY_PATH");
            if (value != null)
            {
                return value;
            }
            throw new ArgumentException(nameof(value), "Unable to find setting 'COSMOS_PARTITION_KEY_PATH'!");
        }
    }

    private string ContainerLeaseId
    {
        get
        {
            var value = Environment.GetEnvironmentVariable("COSMOS_CONTAINER_LEASE_ID");
            if (value != null)
            {
                return value;
            }
            throw new ArgumentException(nameof(value), "Unable to find setting 'COSMOS_CONTAINER_LEASE_ID'!");
        }
    }


    // The Cosmos client instance
    private CosmosClient? _cosmosClient;

    private Database? _database;

    private Container? _container;

    private Container? _leaseContainer;

    public async Task Run()
    {
        await CosmosAsync();
    }

    public async Task CosmosAsync()
    {
        // Create a new instance of the Cosmos Client
        _cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);

        // Runs the CreateDatabaseAsync method
        await CreateDatabaseAsync();

        // Run the CreateContainerAsync method
        await CreateContainerAsync();

        await ProcessFeed();

        await AddItemsToContainerAsync();
    }

    private async Task CreateDatabaseAsync()
    {
        // Create a new database using the cosmosClient
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        _database = await _cosmosClient?.CreateDatabaseIfNotExistsAsync(DatabaseId);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        WriteLine("Created Database: {0}\n", _database.Id);
    }

    private async Task CreateContainerAsync()
    {
        // Create a new container
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        _container = await _database?.CreateContainerIfNotExistsAsync(ContainerId, PartitionKeyPath);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        WriteLine("Created Container: {0}\n", _container.Id);

        _leaseContainer = await _database.CreateContainerIfNotExistsAsync(ContainerLeaseId, "/id");
        WriteLine("Created Container: {0}\n", _leaseContainer.Id);

    }

    private async Task CreateDocumentsIfNotExists(Order order)
    {
        try
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            ItemResponse<Order> readResponse = await _container.ReadItemAsync<Order>(order.id, new PartitionKey(order?.OrderAddress.City));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            WriteLine("Item in database with id: {0} already exists\n", readResponse.Resource.id);
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            ItemResponse<Order> createResponse = await _container?.CreateItemAsync<Order>(order, new PartitionKey(order?.OrderAddress.City));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            WriteLine("Created item in the database with id: {0} Operation consumed {1} RUs.\n", createResponse.Resource.id, createResponse.RequestCharge);
        }
    }

    private async Task ProcessFeed()
    {
        static async Task HandleChangesAsync(
            ChangeFeedProcessorContext context,
            IReadOnlyCollection<Order> changes,
            CancellationToken cancellationToken)
        {
            WriteLine($"Started handling changes for lease {context.LeaseToken}...");
            WriteLine($"Change Feed request consumed {context.Headers.RequestCharge} RU.");
            WriteLine($"SessionToken: {context.Headers.Session}");

            if (context.Diagnostics.GetClientElapsedTime() > TimeSpan.FromSeconds(1))
            {
                WriteLine($"Change Feed request took longer than expected. Diagnostics:" + context.Diagnostics.ToString());
            }

            foreach (Order item in changes)
            {
                WriteLine($"Detected operation for item with id {item.id}.");
                // Simulate some asynchronous operation
                await Task.Delay(10);
            }

            WriteLine("Finished handling changes.");
        }

        Container? sourceContainer = _cosmosClient?.GetContainer(DatabaseId, ContainerId);
        Container? leaseContainer = _cosmosClient?.GetContainer(DatabaseId, ContainerLeaseId);

        var procBuilder = sourceContainer?.GetChangeFeedProcessorBuilder<Order>(
            processorName: "orderItemProcessor",
            onChangesDelegate: HandleChangesAsync
        );

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        ChangeFeedProcessor processor = procBuilder
           .WithInstanceName("TheCloudShops")
           .WithLeaseContainer(leaseContainer)
           .Build();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        await processor.StartAsync();

        ReadKey();
        await processor.StopAsync();
    }

    private async Task AddItemsToContainerAsync()
    {
        Customer customer1 = new() { IsActive = true, Name = "Level4you" };
        Customer customer2 = new() { IsActive = true, Name = "UpperLevel" };
        Customer customer3 = new() { IsActive = false, Name = "Channel-9" };

        Product product1 = new() { ProductName = "Book" };
        Product product2 = new() { ProductName = "Food" };
        Product product3 = new() { ProductName = "Coffee" };

        Order order1 = new()
        {
            id = "o1",
            OrderNumber = "NL-21",
            OrderCustomer = customer1,
            OrderAddress = new Address { State = "WA", County = "King", City = "Seattle" },
            OrderItems = [
                    new OrderItem() { ProductItem  = product1, Count = 7 },
                    new OrderItem() { ProductItem  = product3, Count = 1 }
                ]
        };
        Order order2 = new()
        {
            id = "o2",
            OrderNumber = "NL-22",
            OrderCustomer = customer2,
            OrderAddress = new Address { State = "WA", County = "King", City = "Redmond" },
            OrderItems = [
                    new OrderItem() { ProductItem = product3, Count = 99 },
                    new OrderItem() { ProductItem = product2, Count = 5 },
                    new OrderItem() { ProductItem = product1, Count = 1 }
                ]
        };
        Order order3 = new()
        {
            id = "o3",
            OrderNumber = "NL-23",
            OrderCustomer = customer2,
            OrderAddress = new Address { State = "WA", County = "King", City = "Redmond" },
            OrderItems = [
                    new OrderItem() { ProductItem = product2, Count = 2 }
                ]
        };

        await CreateDocumentsIfNotExists(order1);
        await CreateDocumentsIfNotExists(order2);
        await CreateDocumentsIfNotExists(order3);
    }
}
