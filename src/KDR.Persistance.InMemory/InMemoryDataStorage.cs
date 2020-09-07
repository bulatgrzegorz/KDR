using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KDR.Persistence.Api;

namespace KDR.Persistence.InMemory
{
    public class InMemoryDataStorage : IDataStorage
    {
        private readonly ConcurrentBag<DbMessage> _messagesToSent;

        private readonly ConcurrentBag<ReceivedDbMessage> _receivedMessages;

        private const int BatchSize = 5;
        private const int MaxRetriesCount = 3;

        public InMemoryDataStorage()
        {
            _messagesToSent = new ConcurrentBag<DbMessage>();
            _receivedMessages = new ConcurrentBag<ReceivedDbMessage>();
        }

        public Task < (IEnumerable<object> messages, bool gotMore) > GetMessagesToRetryAsync()
        {
            var messagesToResponse = _messagesToSent
                .Where(x => x.SendDate == null && x.Retries <= MaxRetriesCount)
                .OrderBy(x => x.Id)
                .Take(BatchSize)
                .Cast<object>();

            return Task.FromResult((messagesToResponse, messagesToResponse.Count() == BatchSize));
        }

        public Task MarkMessageAsSendAsync(DbMessage message)
        {
            throw new System.NotImplementedException();
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

        public Task MarkMessageAsFailedAsync(DbMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}