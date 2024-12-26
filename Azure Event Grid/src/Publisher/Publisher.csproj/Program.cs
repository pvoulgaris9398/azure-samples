using Azure;
using Azure.Messaging.EventGrid;
using System;
using System.Threading.Tasks;
using dotenv.net;

namespace publisher
{
    public class Program
    {
        private static string CustomTopicEndpoint
        {
            get
            {
                return Environment.GetEnvironmentVariable("CUSTOM_TOPIC_ENDPOINT") ?? throw new Exception("Environment variable: 'CUSTOM_TOPIC_ENDPOINT' not found!");
            }
        }

        private static string CustomTopicAccessKey
        {
            get
            {
                return Environment.GetEnvironmentVariable("CUSTOM_TOPIC_ACCESS_KEY") ?? throw new Exception("Environment variable: 'CUSTOM_TOPIC_ACCESS_KEY' not found!");
            }
        }

        static async Task Main(string[] args)
        {

            DotEnv.Load();

            while (true)
            {
                Console.WriteLine("Send more events?[Y|N]");
                if (Console.ReadKey(true).Key != ConsoleKey.Y)
                {
                    break;
                }
                Console.WriteLine("\n");
                Console.WriteLine("Enter a valid Ticker:");
                var rawText = Console.ReadLine();
                var ticker = rawText ?? "IBM US";
                Console.WriteLine("Enter the starting quantity:");
                rawText = Console.ReadLine() ?? "1";
                int.TryParse(rawText, out var quantity);
                Console.WriteLine("Enter the resulting quantity:");
                rawText = Console.ReadLine() ?? "1";
                int.TryParse(rawText, out var resultingQuantity);

                await Send(new StockSplit(ticker, quantity, resultingQuantity).ToString());
            }
        }

        private static async Task Send(string eventData)
        {
            EventGridPublisherClient client = new EventGridPublisherClient(
            new Uri(CustomTopicEndpoint),
            new AzureKeyCredential(CustomTopicAccessKey));

            // Add EventGridEvents to a list to publish to the topic
            EventGridEvent egEvent =
                new EventGridEvent(
                    "ExampleEventSubject",
                    "Example.EventType",
                    "1.0",
                    eventData);

            // Send the event
            await client.SendEventAsync(egEvent);
        }
    }
}

