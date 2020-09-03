using System;
using System.Threading.Tasks;
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

        public InMemorySendingDispatcher(IDispatcher dispatcher, ITransportSenderClient transportSenderClient, ILogger<InMemorySendingDispatcher> logger)
        {
            _dispatcher = dispatcher;
            _transportSenderClient = transportSenderClient;
            _logger = logger;
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
                await _transportSenderClient.SendAsync(new TransportMessage(null, null), context.CancellationToken);
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