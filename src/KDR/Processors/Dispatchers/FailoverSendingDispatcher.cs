using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using KDR.Persistence.Api;
using KDR.Transport.Api;

namespace KDR.Processors.Dispatchers
{
    public class FailoverSendingDispatcher : IProcessor
    {
        private readonly IDataStorage _dataStorage;
        private readonly ITransportSenderClient _transportSenderClient;

        public FailoverSendingDispatcher(IDataStorage dataStorage, ITransportSenderClient transportSenderClient)
        {
            _dataStorage = dataStorage;
            _transportSenderClient = transportSenderClient;
        }

        public async Task<bool> ProcessAsync(ProcessingContext context)
        {
            var storageResult = await _dataStorage.GetMessagesToRetryAsync();

            foreach (var messageToRetry in storageResult.messages)
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

            //TODO: Trzeba wykorzystać flagę gotMore żeby przekazać ją niżej.
            return true;
        }
    }
}