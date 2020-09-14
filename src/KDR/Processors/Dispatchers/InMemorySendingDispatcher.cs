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

            (Messages.Message message, Guid dbMessageId) = await _dispatcher.ReadQueuedToPublishAsync(context.CancellationToken);
            try
            {
                //TODO: To można wyciągnąć do osobnej abstrakcji, będzie używane w conajmniej 2 miejscach.
                var result = await _transportSenderClient.SendAsync(new TransportMessage(message.Headers, PrepareBody(message.Body)), context.CancellationToken);
                if (result)
                {
                    await _dataStorage.MarkMessageAsSendAsync(dbMessageId);
                }
                else
                {
                    await _dataStorage.MarkMessageAsFailedAsync(dbMessageId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An exception occurred when sending a message to the MQ. Id:{1}");
                throw;
            }

            return true;
        }

        //TODO: Jak to zrobić? Generalnie serializacja już zaszła podczas zapisu do bazy, więc nie trzeba by było wykonywać jej ponowanie
        //wystarczy przejąć obiekt stworzony tam i przekaząć do publishera, może potrzebujemy obiektu jak Message ale przez wysyłką? 
        private byte[] PrepareBody(object obj){
            return new byte[0];
        }
    }
}