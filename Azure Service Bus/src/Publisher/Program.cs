using System.Text.Json;
using System.Transactions;
using Azure.Messaging.ServiceBus;
using Domain;

namespace Publisher;

public class Program
{
    private static readonly string _firstQueueName = "simple-queue";
    private static readonly string _secondQueueName = "advanced-queue";
    private static readonly string[] _citylist = new[] { "Miami", "NYC", "London", "Paris", "Caracas" };

    private static async Task Main()
    {
        // configure client
        var client = new ServiceBusClient(ConnectionString,
            new ServiceBusClientOptions { EnableCrossEntityTransactions = true });

        var firstSender = client.CreateSender(_firstQueueName);
        var secondSender = client.CreateSender(_secondQueueName);

        var i = 0;
        foreach (var city in _citylist)
        {
            //submitting messages in transactions
            using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // serialize booking object
                var msg = JsonSerializer.Serialize(new Booking
                {
                    HotelBookings = new[]
                    {
                        new Booking.HotelBooking
                            { City = city, CheckinDate = DateTime.Now, LeaveDate = DateTime.Now.AddDays(1) }
                    }
                });


                var messageOne = new ServiceBusMessage(msg);
                //send in first queue
                await firstSender.SendMessageAsync(messageOne);

                var messageTwo = new ServiceBusMessage(msg)
                {
                    SessionId = "transaction-demo"
                };

                //send in second queue
                await secondSender.SendMessageAsync(messageTwo);

                //commit
                ts.Complete();
            }

            Console.WriteLine($"Message #{++i} was sent in both queue");
        }

        //release recourses
        await firstSender.CloseAsync();
        await secondSender.CloseAsync();
    }
}