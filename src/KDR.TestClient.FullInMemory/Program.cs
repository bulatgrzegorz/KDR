using System;
using System.Transactions;
using Microsoft.Extensions.DependencyInjection;
using KDR.Transactions;
using KDR.Processors.Receivers;
using KDR.Processors.Outgoing;
using System.Threading.Tasks;
using KDR.Abstractions.Messages;
using KDR.Persistence.Api;
using KDR.Persistence.InMemory;

namespace KDR.TestClient.FullInMemory
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var sc = new ServiceCollection();

            KDR.CreateEventBus(sc);
            sc.AddSingleton<IDataStorage, InMemoryDataStorage>();

            var sp = sc.BuildServiceProvider();

            var eb = sp.GetRequiredService<IEventBus>();
            await eb.PublishEventAsync(new Event());
        }
    }

    public class Event : IEvent{

    }
}
