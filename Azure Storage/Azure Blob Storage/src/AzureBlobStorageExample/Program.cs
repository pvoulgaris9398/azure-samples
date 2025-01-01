using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

namespace AzureBlobStorageExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Test");
        }


        public BlobServiceClient GetBlobServiceClient(string accountName)
        {
            BlobServiceClient client = new(
                new Uri("https://{accountName}.blob.core.windows.net"),
                new DefaultAzureCredential());
            return client;

        }
    }
}