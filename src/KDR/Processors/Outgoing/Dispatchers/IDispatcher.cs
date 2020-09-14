using System;
using System.Threading;
using System.Threading.Tasks;
namespace KDR.Processors.Outgoing.Dispatchers
{
    //TODO: To trzeba nazwać jakoś inaczej 
    public interface IDispatcher
    {
        ValueTask EnqueueToPublishAsync(Messages.Message message, Guid dbMessageId);

        void EnqueueToPublish(Messages.Message message, Guid dbMessageId);

        ValueTask<bool> WaitToReadQueuedToPublishAsync(CancellationToken cancellationToken);

        ValueTask<(Messages.Message message, Guid dbMessageId)> ReadQueuedToPublishAsync(CancellationToken cancellationToken);
    }
}