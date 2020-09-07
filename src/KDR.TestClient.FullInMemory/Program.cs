using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Transactions;
using KDR.Abstractions.Messages;
using KDR.Persistence.Api;
using KDR.Persistence.InMemory;
using KDR.Processors;
using KDR.Processors.Outgoing;
using KDR.Processors.Receivers;
using KDR.Transactions;
using KDR.Transport.Api;
using KDR.Transport.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KDR.TestClient.FullInMemory
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var sc = new ServiceCollection();
            sc.AddLogging(c => c.AddConsole());
            KDR.CreateEventBus(sc);
            sc.AddSingleton<IDataStorage, InMemoryDataStorage>();
            sc.AddSingleton<ITransportSenderClient, InMemorySenderClient>();
            sc.AddSingleton<IProcessorsManager, ProcessorsManager>();
            var sp = sc.BuildServiceProvider();

            var pm = sp.GetRequiredService<IProcessorsManager>();
            var eb = sp.GetRequiredService<IEventBus>();

            await pm.StartAsync();

            using(var ts = new TransactionScope())
            {
                await eb.PublishEventAsync(new Event());

                ts.Complete();
            }

            do
            {
                var key = Console.ReadLine();
                if (key == "p")
                {
                    await eb.PublishEventAsync(new Event());
                }
                else if (key == "d")
                {
                    await pm.DisposeAsync();
                }
            } while (true);
        }
    }

    public class Event : IEvent
    {

    }
}