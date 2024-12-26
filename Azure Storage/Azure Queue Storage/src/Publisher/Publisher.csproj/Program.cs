using System;
using Azure.Storage.Queues;
using System.Text.Json;
using dotenv.net;

namespace Publisher
{
    //custom class to serialize in the message
    public class TheMessage
    {
        public string MsgID { get; set; } = "Test1";
        public string Info { get; set; } = "Test2";
    }

    class Program
    {
        private static string ConnectionString
        {
            get
            {
                return Environment.GetEnvironmentVariable("AZURE_QUEUE_STORAGE_CONNECTION_STRING") ?? throw new Exception("Environment variable: 'AZURE_QUEUE_STORAGE_CONNECTION_STRING' not found!");
            }
        }

        private static string QueueName
        {
            get
            {
                return Environment.GetEnvironmentVariable("AZURE_QUEUE_STORAGE_QUEUE_NAME") ?? throw new Exception("Environment variable: 'AZURE_QUEUE_STORAGE_QUEUE_NAME' not found!");
            }
        }

        static void Main(string[] args)
        {
            DotEnv.Load();

            Console.WriteLine("Your publisher just started!");

            var client = CreateQueueClient(); //build client

            // sending 30 messages 
            for (var i = 0; i < 30; i++)
            {
                string msg = JsonSerializer.Serialize(new TheMessage() { MsgID = $"{i}", Info = $"Simple messaging #{i}" });
                InsertMessage(client, msg);
            }
        }

        public static QueueClient CreateQueueClient()
        {
            try
            {

                QueueClient queueClient = new QueueClient(ConnectionString, QueueName);

                queueClient.CreateIfNotExists();    // Create the queue

                if (queueClient.Exists())
                    Console.WriteLine($"The queue created: '{queueClient.Name}'");

                return queueClient;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}\n\n");
                throw;
            }
        }


        public static void InsertMessage(QueueClient queueClient, string message)
        {
            //sending messages from 
            queueClient.SendMessage(message);

            Console.WriteLine($"Message inserted");
        }
    }
}
