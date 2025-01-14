﻿using Azure.Messaging.ServiceBus;
using System.Text.Json;
using Azure.Messaging.ServiceBus.Administration;
using System.Transactions;
using Domain;

namespace Consumer
{
    public class Program
    {
        private static string _advQueue = "advanced-queue";
        private static string _simplQueue = "simple-queue";

        static async Task Main(string[] args)
        {
            // configure admin client
            ServiceBusAdministrationClient adminClint = new ServiceBusAdministrationClient(ConnectionString);
            // configure transaction client
            ServiceBusClient client = new ServiceBusClient(ConnectionString, new ServiceBusClientOptions { EnableCrossEntityTransactions = true });
            //configure non-transaction client
            ServiceBusClient nonTransactionClient = new ServiceBusClient(ConnectionString);

            while (true)
            {
                QueueRuntimeProperties advRuntimeProp = await adminClint.GetQueueRuntimePropertiesAsync(_advQueue);
                QueueRuntimeProperties simplRuntimeProp = await adminClint.GetQueueRuntimePropertiesAsync(_simplQueue);

                Console.WriteLine($"\r\n{_advQueue} messages count: {advRuntimeProp.ActiveMessageCount}");
                Console.WriteLine($"{_simplQueue} messages count: {simplRuntimeProp.ActiveMessageCount}");
                Console.WriteLine($"DLQ messages count: {advRuntimeProp.DeadLetterMessageCount}");

                Console.WriteLine("\r\nChose [1-4] for demonstration:");
                Console.WriteLine("\t1 - Receive and Send in transaction");
                Console.WriteLine("\t2 - Used Dead-letter queue");
                Console.WriteLine("\t3 - Receive from Dead-letter queue");
                Console.WriteLine("\t4 - Batch Peek all messages");
                Console.WriteLine("Chose [1-4]");

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        await ReceiveInTransaction(client);
                        break;
                    case ConsoleKey.D2:
                        await FailReceive(nonTransactionClient);
                        break;
                    case ConsoleKey.D3:
                        await ReceiveDeadLetter(nonTransactionClient);
                        break;
                    case ConsoleKey.D4:
                        await PeekAsync(nonTransactionClient);
                        break;
                }
            }
        }

        /// <summary>
        /// The function pull messages from DLQ
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static async Task ReceiveDeadLetter(ServiceBusClient client)
        {
            //create a receiver pointed to DLQ
            ServiceBusReceiver dlqReceiver = client.CreateReceiver(_advQueue, new ServiceBusReceiverOptions
            {
                SubQueue = SubQueue.DeadLetter
            });

            //pull msg
            ServiceBusReceivedMessage dlqMessage = await dlqReceiver.ReceiveMessageAsync(maxWaitTime: TimeSpan.FromSeconds(3));

            if (dlqMessage != null)
            {
                // processing message....
                var booking = JsonSerializer.Deserialize<Booking>(dlqMessage.Body);
                Console.WriteLine($"DQL Message received:\r\n{booking}");
                // delete messages from the queue
                await dlqReceiver.CompleteMessageAsync(dlqMessage);
            }
            //release
            await dlqReceiver.CloseAsync();
        }

        /// <summary>
        /// The function will list all msg in both queue
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static async Task PeekAsync(ServiceBusClient client)
        {

            foreach (var queName in new[] { _simplQueue, _advQueue })
            {
                //create receiver
                ServiceBusReceiver receiver = client.CreateReceiver(queName);

                //pull batch of msg
                var messages = await receiver.PeekMessagesAsync(32);

                Console.WriteLine($"\r\nMessage in queue '{queName}'");

                foreach (var msg in messages)
                {
                    var booking = JsonSerializer.Deserialize<Booking>(msg.Body);
                    Console.WriteLine($"Message (sessionId: {msg.SessionId})  peeked:\r\n{booking}");
                }

                //release resource
                await receiver.CloseAsync();
            }
        }


        /// <summary>
        /// This function will demonstrate pumping messages from simple to advanced queue
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static async Task ReceiveInTransaction(ServiceBusClient client)
        {
            //create sender for advanced queue
            ServiceBusSender sender = client.CreateSender(_advQueue);
            //create receiver for simple queue
            ServiceBusReceiver receiver = client.CreateReceiver(_simplQueue);

            //pull msg from simple queue
            var firstMsg = await receiver.ReceiveMessageAsync(maxWaitTime: TimeSpan.FromSeconds(3));

            if (firstMsg != null)
            {
                // processing message....
                var booking = JsonSerializer.Deserialize<Booking>(firstMsg.Body);
                Console.WriteLine($"Adv Message received:\r\n{booking}");

                //start transaction
                using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    // crate a msg for advanced queue with session support
                    ServiceBusMessage secondMsg = new ServiceBusMessage(firstMsg) { SessionId = "transaction-demo" };

                    //send msg
                    await sender.SendMessageAsync(secondMsg);

                    Console.WriteLine($"Msg submitted in adv queue.");

                    //delete msg in simple queue
                    await receiver.CompleteMessageAsync(firstMsg);

                    //commit transaction
                    ts.Complete();
                }
            }


            //release session
            await receiver.CloseAsync();
            await sender.CloseAsync();
        }


        /// <summary>
        /// The function will demonstrate how to use Dead-letter queue for poison msg
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static async Task FailReceive(ServiceBusClient client)
        {
            //create receiver
            ServiceBusReceiver receiver = await client.AcceptNextSessionAsync(_advQueue);

            //pull a message
            var msg = await receiver.ReceiveMessageAsync(maxWaitTime: TimeSpan.FromSeconds(3));

            // processing message....
            var booking = JsonSerializer.Deserialize<Booking>(msg.Body);
            Console.WriteLine($"Message received:\r\n{booking}");

            try
            {
                //possible nullref exception
                Console.WriteLine($"booking contains flights: {booking?.AirBookings?.Length} , hotels: {booking?.HotelBookings?.Length}");
            }
            catch (Exception)
            {
                Console.WriteLine("Exception during processing, move msg to DQL");

                //can not properly process the message and move it DQL
                await receiver.DeadLetterMessageAsync(msg);
            }

            //release session
            await receiver.CloseAsync();
        }


    }

}

