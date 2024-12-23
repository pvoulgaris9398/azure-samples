using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Primitives;
using Azure.Storage.Blobs;
using dotenv.net;
using Azure.Messaging.EventHubs.Processor;

namespace Subscriber
{
    public class Program
    {


        /// <summary>
        /// EVENT_HUB_NAMESPACE_NAME=eventhub4991
        /// EVENT_HUB_SHARED_ACCESS_KEY_NAME=app
        /// EVENT_HUB_SHARED_ACCESS_KEY=***
        /// EVENT_HUB_NAME=eventhub4991
        /// Note:
        /// EVENT_HUB_NAMESPACE_NAME and EVENT_HUB_NAME are the same, but they don't have to and probably _shouldn't_ be the same
        /// "Endpoint=sb://{EVENT_HUB_NAMESPACE_NAME}.servicebus.windows.net/;SharedAccessKeyName={EVENT_HUB_SHARED_ACCESS_KEY_NAME};SharedAccessKey={EVENT_HUB_SHARED_ACCESS_KEY};EntityPath={EVENT_HUB_NAME}"
        /// </summary>        
        private static string EventHubConnectionString
        {
            get
            {
                return Environment.GetEnvironmentVariable("EVENT_HUB_CONNECTION_STRING") ?? throw new Exception("Environment variable: 'EVENT_HUB_CONNECTION_STRING' not found!");
            }
        }

        /// <summary>
        /// STORAGE_ACCOUNT_KEY=****
        /// STORAGE_ACCOUNT_NAME=eventhub4991
        /// "DefaultEndpointsProtocol=https;AccountName={STORAGE_ACCOUNT_NAME};AccountKey={STORAGE_ACCOUNT_KEY};EndpointSuffix=core.windows.net"
        /// </summary>
        private static string StorageConnectionString
        {
            get
            {
                return Environment.GetEnvironmentVariable("STORAGE_ACCOUNT_CONNECTION_STRING") ?? throw new Exception("Environment variable: 'STORAGE_ACCOUNT_CONNECTION_STRING' not found!");
            }
        }

        private static async Task Main(string[] args)
        {

            DotEnv.Load();

            var storageClient = new BlobContainerClient(StorageConnectionString, "checkpoint");

            var processor = new SimpleEventProcessor(
                    storageClient, 1, "$Default",
                    EventHubConnectionString);

            using var cancellationSource = new CancellationTokenSource();
            cancellationSource.CancelAfter(TimeSpan.FromSeconds(60));

            try
            {
                await processor.StartProcessingAsync(cancellationSource.Token);
                await Task.Delay(Timeout.Infinite, cancellationSource.Token);

                Console.WriteLine("Receiving. Press key to stop worker.");
                Console.ReadKey();
            }
            catch (TaskCanceledException)
            {
                // This is expected if the cancellation token is
                // signaled.
                await processor.StopProcessingAsync();
            }
            finally
            {
                // Stopping may take up to the length of time defined
                // as the TryTimeout configured for the processor;
                // By default, this is 60 seconds.

                await processor.StopProcessingAsync();
            }

        }
    }

    public class SimpleEventProcessor : PluggableCheckpointStoreEventProcessor<EventProcessorPartition>
    {
        // This example uses a connection string, so only the single constructor
        // was implemented; applications will need to shadow each constructor of
        // the PluggableCheckpointStoreEventProcessor that they are using.

        public SimpleEventProcessor(
            BlobContainerClient storageClient,
            int eventBatchMaximumCount,
            string consumerGroup,
            string connectionString,
            EventProcessorOptions? clientOptions = default)
                : base(
                    new BlobCheckpointStore(storageClient),
                    eventBatchMaximumCount,
                    consumerGroup,
                    connectionString,
                    clientOptions)
        {
        }

        protected async override Task OnProcessingEventBatchAsync(
            IEnumerable<EventData> messages,
            EventProcessorPartition partition,
            CancellationToken cancellationToken)
        {
            EventData? lastEvent = null;
            try
            {
                foreach (var currentEvent in messages)
                {
                    lastEvent = currentEvent;
                    var data = currentEvent.EventBody;
                    Console.WriteLine($"Event received. Partition: '{partition.PartitionId}', Data: '{data}'");

                }

                if (lastEvent != null)
                {
                    await UpdateCheckpointAsync(
                        partition.PartitionId,
                        CheckpointPosition.FromEvent(lastEvent),
                        cancellationToken)
                    .ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while processing events: {ex}");
            }
        }

        protected override Task OnProcessingErrorAsync(
            Exception exception,
            EventProcessorPartition partition,
            string operationDescription,
            CancellationToken cancellationToken)
        {
            try
            {
                if (partition != null)
                {
                    Console.WriteLine($"Error on Partition: {partition.PartitionId}, Error: {operationDescription}: {exception}");
                }
                else
                {
                    Console.Error.WriteLine(
                        $"Exception while performing {operationDescription}: {exception}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while processing events: {ex}");
            }

            return Task.CompletedTask;
        }

        protected override Task OnInitializingPartitionAsync(
            EventProcessorPartition partition,
            CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine($"Initializing partition {partition.PartitionId}");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Exception while initializing a partition: {ex}");
            }

            return Task.CompletedTask;
        }

        protected override Task OnPartitionProcessingStoppedAsync(
            EventProcessorPartition partition,
            ProcessingStoppedReason reason,
            CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine(
                    $"No longer processing partition {partition.PartitionId} because {reason}");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Exception while stopping processing for a partition: {ex}");
            }

            return Task.CompletedTask;
        }
    }
}