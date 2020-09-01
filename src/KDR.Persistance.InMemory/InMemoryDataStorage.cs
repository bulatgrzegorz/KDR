using System.Collections.Concurrent;
using System.Threading.Tasks;
using KDR.Persistence.Api;

namespace KDR.Persistence.InMemory
{
    public class InMemoryDataStorage : IDataStorage
    {
        private readonly ConcurrentBag<DbMessage> _messagesToSent;

        private readonly ConcurrentBag<ReceivedDbMessage> _receivedMessages;

        public InMemoryDataStorage()
        {
            _messagesToSent = new ConcurrentBag<DbMessage>();
            _receivedMessages = new ConcurrentBag<ReceivedDbMessage>();
        }

        public Task StoreMessageToSendAsync(DbMessage message)
        {
            _messagesToSent.Add(message);

            return Task.CompletedTask;
        }

        public Task<int?> StoreReceivedMessageAsync(ReceivedDbMessage message)
        {
            _receivedMessages.Add(message);

            return Task.FromResult<int?>(1);
        }
    }
}
