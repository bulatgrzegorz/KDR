using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KDR.Persistence.Api;
using KDR.Serialization;

namespace KDR.Persistence.InMemory
{
    public class InMemoryDataStorage : IDataStorage
    {
        private readonly ISerializerFactory _serialzationFactory;

        private readonly ConcurrentBag<DbMessage> _messagesToSent;

        private readonly ConcurrentBag<ReceivedDbMessage> _receivedMessages;

        private const int BatchSize = 5;
        private const int MaxRetriesCount = 3;

        public InMemoryDataStorage(ISerializerFactory serialzationFactory)
        {
            _messagesToSent = new ConcurrentBag<DbMessage>();
            _receivedMessages = new ConcurrentBag<ReceivedDbMessage>();
            _serialzationFactory = serialzationFactory;
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

        public async Task<DbMessage> StoreMessageToSendAsync(object body, IDictionary<string, string> headers)
        {
            var content = await _serialzationFactory.Default.SerializeAsync(body, headers); 
            
            var dbMessage = new DbMessage(){
                Created = DateTime.Now,
                Id = Guid.NewGuid(),
                Content = content.serializedBody,
                Headers = content.serializedHeaders
            };

            _messagesToSent.Add(dbMessage);

            return dbMessage;
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