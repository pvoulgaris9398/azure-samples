using Azure.Messaging.ServiceBus;

internal class Program
{
        [STAThread]
        private static async void Main(string[] args)
        {

            // connection string to your Service Bus namespace
            string connectionString = "Endpoint=sb://az204svcbus9398.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=K/XScaT/QpVtMj+TUil+EaS0CiwBhN5WE+ASbGw9+oc=";

            // name of your Service Bus topic
            string queueName = "az204queue";

            // the client that owns the connection and can be used to create senders and receivers
            ServiceBusClient client;

            // the sender used to publish messages to the queue
            ServiceBusSender sender;

            // Create the clients that we'll use for sending and processing messages.
            client = new ServiceBusClient(connectionString);
            sender = client.CreateSender(queueName);

            // create a batch 
            using (ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync())
            {

                for (int i = 1; i <= 3; i++)
                {
                    // try adding a message to the batch
                    if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
                    {
                        // if an exception occurs
                        throw new Exception($"Exception {i} has occurred.");
                    }
                }

                try
                {
                    // Use the producer client to send the batch of messages to the Service Bus queue
                    await sender.SendMessagesAsync(messageBatch);
                    Console.WriteLine($"A batch of three messages has been published to the queue.");
                }
                finally
                {
                    // Calling DisposeAsync on client types is required to ensure that network
                    // resources and other unmanaged objects are properly cleaned up.
                    await sender.DisposeAsync();
                    await client.DisposeAsync();
                }
            }

            Console.WriteLine("Follow the directions in the exercise to review the results in the Azure portal.");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        
    }
}
//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
