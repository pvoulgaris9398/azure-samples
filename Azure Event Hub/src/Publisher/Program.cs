using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using dotenv.net;

namespace Publisher
{
    public class Program
    {
        private static EventHubProducerClient? client;

        private static string EventHubConnectionString
        {
            get
            {
                return Environment.GetEnvironmentVariable("EVENT_HUB_CONNECTION_STRING") ?? throw new Exception("Environment variable: 'EVENT_HUB_CONNECTION_STRING' not found!");
            }
        }

        static async Task Main(string[] args)
        {
            DotEnv.Load();

            client = new EventHubProducerClient(EventHubConnectionString);

            while (true)
            {
                Console.WriteLine("Send more events?[Y|N]");
                if (Console.ReadKey(true).Key != ConsoleKey.Y)
                {
                    break;
                }
                Console.WriteLine("Enter a message to send:");
                var messageText = Console.ReadLine() ?? "No Text";
                await SendEventsToEventHubAsync(10, messageText);
            }

            await client.CloseAsync();

        }

        // Creates an Event Hub client and sends 10 messages to the event hub.
        static async Task SendEventsToEventHubAsync(int numMsgToSend, string messageText)
        {
            for (var i = 0; i < numMsgToSend; i = i + 2)
            {
                //Create batch with 2 events
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                using EventDataBatch eventBatch = await client.CreateBatchAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                {
                    for (var j = 0; j < 2; j++)
                    {

                        try
                        {
                            var message = $"Event #{i + j} (Message={messageText})";

                            var eventData = new EventData(new BinaryData(message));
                            Console.WriteLine($"Sending event: {message}");
                            eventBatch.TryAdd(eventData);
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                        }
                    }

                }

                await Task.Delay(10);
                await client.SendAsync(eventBatch);

            }

            Console.WriteLine($"{numMsgToSend} events sent.");
        }
    }
}