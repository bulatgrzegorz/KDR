using System.Security.Cryptography.X509Certificates;
using System;
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
        private static int _messagesToSendActualId = 0;

        private readonly ConcurrentBag<ReceivedDbMessage> _receivedMessages;

        private const int BatchSize = 5;
        private const int MaxRetriesCount = 3;

        public InMemoryDataStorage()
        {
            _messagesToSent = new ConcurrentBag<DbMessage>();
            _receivedMessages = new ConcurrentBag<ReceivedDbMessage>();
        }

        public Task < (IEnumerable<DbMessage> messages, bool gotMore) > GetMessagesToRetryAsync()
        {
            var messagesToResponse = _messagesToSent
                .Where(x => x.SendDate == null && x.Retries <= MaxRetriesCount)
                .OrderBy(x => x.Id)
                .Take(BatchSize);

            return Task.FromResult((messagesToResponse, messagesToResponse.Count() == BatchSize));
        }

        public Task<int?> StoreReceivedMessageAsync(ReceivedDbMessage message)
        {
            _receivedMessages.Add(message);

            return Task.FromResult<int?>(1);
        }

        public Task<DbMessage> StoreMessageToSendAsync(object Body, IDictionary<string, string> headers)
        {
            var dbMessage = new DbMessage(){
                Created = DateTime.Now
            };

            _messagesToSent.Add(dbMessage);

            return Task.FromResult(dbMessage);
        }

        public Task MarkMessageAsSendAsync(Guid messageId)
        {
            var message = _messagesToSent.SingleOrDefault(x => x.Id == messageId);
            if(message != null)
            {
                message.SendDate = DateTime.Now;
            }

            return Task.CompletedTask;
        }

        public Task MarkMessageAsFailedAsync(Guid messageId)
        {
            var message = _messagesToSent.SingleOrDefault(x => x.Id == messageId);
            if(message != null)
            {
                message.Retries += 1;
            }

            return Task.CompletedTask;
        }
    }
}