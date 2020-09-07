using System;
using System.Threading.Tasks;
using KDR.Persistence.Api;
using KDR.Processors.Outgoing.Dispatchers;
using KDR.Transport.Api;
using Microsoft.Extensions.Logging;

namespace KDR.Processors.Dispatchers
{
    public class InMemorySendingDispatcher : IProcessor
    {
        private readonly IDispatcher _dispatcher;
        private readonly ITransportSenderClient _transportSenderClient;
        private readonly ILogger<InMemorySendingDispatcher> _logger;
        private readonly IDataStorage _dataStorage;

        public InMemorySendingDispatcher(IDispatcher dispatcher, ITransportSenderClient transportSenderClient, ILogger<InMemorySendingDispatcher> logger, IDataStorage dataStorage)
        {
            _dispatcher = dispatcher;
            _transportSenderClient = transportSenderClient;
            _logger = logger;
            _dataStorage = dataStorage;
        }

        public async Task<bool> ProcessAsync(ProcessingContext context)
        {
            var awaitedQueueResult = await _dispatcher.WaitToReadQueuedToPublishAsync(context.CancellationToken);
            if (!awaitedQueueResult)
            {
                return false;
            }

            var message = await _dispatcher.ReadQueuedToPublishAsync(context.CancellationToken);
            if (message == null)
            {
                //TODO: to się może wydarzyć? 
                return true;
            }
            try
            {
                //TODO: To można wyciągnąć do osobnej abstrakcji, będzie używane w conajmniej 2 miejscach.
                var result = await _transportSenderClient.SendAsync(new TransportMessage(null, null), context.CancellationToken);
                if (result)
                {
                    await _dataStorage.MarkMessageAsSendAsync(new DbMessage());
                }
                else
                {
                    await _dataStorage.MarkMessageAsFailedAsync(new DbMessage());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An exception occurred when sending a message to the MQ. Id:{1}");
                throw;
            }

            return true;
        }
    }
}