using Azure.Identity;
using Azure.Storage.Blobs;
using DotNetEnv;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure;

namespace AzureBlobStorageExample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Env.Load();

            var accountName = Environment.GetEnvironmentVariable("BLOB_ACCOUNT_NAME") ?? throw new Exception("Could not find environment variable!");

            var blobServiceClient = GetBlobServiceClient(accountName);

            var containerName = Environment.GetEnvironmentVariable("BLOB_CONTAINER_NAME") ?? throw new Exception("Could not find environment variable!");

            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            containerClient.CreateIfNotExists();

            await AddContainerMetadata(containerClient, "Environment", "dev");

            DisplayContainerProperties(containerClient);

            await AddContainerMetadata(containerClient, "Environment", "staging");

            DisplayContainerProperties(containerClient);



            await ListContainers(blobServiceClient, "cont", null);

        }

        private static async Task AddContainerMetadata(BlobContainerClient client,
        string name,
        string value)
        {
            IDictionary<string, string> metadata =
           new Dictionary<string, string>();

            // Add some metadata to the container.
            metadata.Add("docType", "textDocuments");
            metadata.Add("category", "guidance");
            metadata.Add(name, value);

            // Set the container's metadata.

            await client.SetMetadataAsync(metadata);
        }

        private static void DisplayContainerProperties(BlobContainerClient client)
        {
            var properties = client.GetProperties().Value;

            Console.WriteLine("{0}: {1}", nameof(client.AccountName), client.AccountName);
            Console.WriteLine("{0}: {1}", nameof(client.Name), client.Name);
            Console.WriteLine("{0}: {1}", nameof(properties.ETag), properties.ETag);
            Console.WriteLine("{0}: {1}", nameof(properties.LastModified), properties.LastModified);
            foreach (var property in properties.Metadata)
            {
                Console.WriteLine("{0}: {1}", property.Key, property.Value);

            }
        }
        private async static Task ListContainers(BlobServiceClient blobServiceClient,
                                        string prefix,
                                        int? segmentSize)
        {
            try
            {
                // Call the listing operation and enumerate the result segment.
                var resultSegment =
                    blobServiceClient.GetBlobContainersAsync(BlobContainerTraits.Metadata, prefix, default)
                    .AsPages(default, segmentSize);

                await foreach (Azure.Page<BlobContainerItem> containerPage in resultSegment)
                {
                    foreach (BlobContainerItem containerItem in containerPage.Values)
                    {
                        Console.WriteLine("Container name: {0}", containerItem.Name);
                    }

                    Console.WriteLine();
                }
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        private static BlobServiceClient GetBlobServiceClient(string accountName)
        {
            BlobServiceClient client = new(
                new Uri($"https://{accountName}.blob.core.windows.net"),
                new DefaultAzureCredential());
            return client;

        }
    }
}