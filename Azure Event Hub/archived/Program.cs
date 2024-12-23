// See https://aka.ms/new-console-template for more information
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Messaging.EventHubs.Producer;
using Azure.Storage.Blobs;

Console.WriteLine("Hello, World!");

static async void InspectEventHubs()
{
    var connectionString = "<< CONNECTION STRING FOR THE EVENT HUBS NAMESPACE >>";
    var eventHubName = "<< NAME OF THE EVENT HUB >>";

    await using (var producer = new EventHubProducerClient(connectionString, eventHubName))
    {
        using EventDataBatch eventBatch = await producer.CreateBatchAsync();
        eventBatch.TryAdd(new EventData(new BinaryData("First")));
        eventBatch.TryAdd(new EventData(new BinaryData("Second")));

        await producer.SendAsync(eventBatch);
    }
}

static async void PublishEvents()
{
    var connectionString = "<< CONNECTION STRING FOR THE EVENT HUBS NAMESPACE >>";
    var eventHubName = "<< NAME OF THE EVENT HUB >>";

    await using (var producer = new EventHubProducerClient(connectionString, eventHubName))
    {
        using EventDataBatch eventBatch = await producer.CreateBatchAsync();
        eventBatch.TryAdd(new EventData(new BinaryData("First")));
        eventBatch.TryAdd(new EventData(new BinaryData("Second")));

        await producer.SendAsync(eventBatch);
    }
}


static async void ReadEventsFromAnEventHubs()
{
    var connectionString = "<< CONNECTION STRING FOR THE EVENT HUBS NAMESPACE >>";
    var eventHubName = "<< NAME OF THE EVENT HUB >>";

    string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

    await using (var consumer = new EventHubConsumerClient(consumerGroup, connectionString, eventHubName))
    {
        using var cancellationSource = new CancellationTokenSource();
        cancellationSource.CancelAfter(TimeSpan.FromSeconds(45));

        await foreach (PartitionEvent receivedEvent in consumer.ReadEventsAsync(cancellationSource.Token))
        {
            // At this point, the loop will wait for events to be available in the Event Hub.  When an event
            // is available, the loop will iterate with the event that was received.  Because we did not
            // specify a maximum wait time, the loop will wait forever unless cancellation is requested using
            // the cancellation token.
        }
    }
}

static async void ReadEventsFromAnEventHubsPartition()
{
    var connectionString = "<< CONNECTION STRING FOR THE EVENT HUBS NAMESPACE >>";
    var eventHubName = "<< NAME OF THE EVENT HUB >>";

    string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

    await using (var consumer = new EventHubConsumerClient(consumerGroup, connectionString, eventHubName))
    {
        EventPosition startingPosition = EventPosition.Earliest;
        string partitionId = (await consumer.GetPartitionIdsAsync()).First();

        using var cancellationSource = new CancellationTokenSource();
        cancellationSource.CancelAfter(TimeSpan.FromSeconds(45));

        await foreach (PartitionEvent receivedEvent in consumer.ReadEventsFromPartitionAsync(partitionId, startingPosition, cancellationSource.Token))
        {
            // At this point, the loop will wait for events to be available in the partition.  When an event
            // is available, the loop will iterate with the event that was received.  Because we did not
            // specify a maximum wait time, the loop will wait forever unless cancellation is requested using
            // the cancellation token.
        }
    }
}

static async void ProcessEventsWithEventProcessorClient()
{
    var cancellationSource = new CancellationTokenSource();
    cancellationSource.CancelAfter(TimeSpan.FromSeconds(45));

    var storageConnectionString = "<< CONNECTION STRING FOR THE STORAGE ACCOUNT >>";
    var blobContainerName = "<< NAME OF THE BLOB CONTAINER >>";

    var eventHubsConnectionString = "<< CONNECTION STRING FOR THE EVENT HUBS NAMESPACE >>";
    var eventHubName = "<< NAME OF THE EVENT HUB >>";
    var consumerGroup = "<< NAME OF THE EVENT HUB CONSUMER GROUP >>";

    Task processEventHandler(ProcessEventArgs eventArgs) => Task.CompletedTask;
    Task processErrorHandler(ProcessErrorEventArgs eventArgs) => Task.CompletedTask;

    var storageClient = new BlobContainerClient(storageConnectionString, blobContainerName);
    var processor = new EventProcessorClient(storageClient, consumerGroup, eventHubsConnectionString, eventHubName);

    processor.ProcessEventAsync += processEventHandler;
    processor.ProcessErrorAsync += processErrorHandler;

    await processor.StartProcessingAsync();

    try
    {
        // The processor performs its work in the background; block until cancellation
        // to allow processing to take place.

        await Task.Delay(Timeout.Infinite, cancellationSource.Token);
    }
    catch (TaskCanceledException)
    {
        // This is expected when the delay is canceled.
    }

    try
    {
        await processor.StopProcessingAsync();
    }
    finally
    {
        // To prevent leaks, the handlers should be removed when processing is complete.

        processor.ProcessEventAsync -= processEventHandler;
        processor.ProcessErrorAsync -= processErrorHandler;
    }
}