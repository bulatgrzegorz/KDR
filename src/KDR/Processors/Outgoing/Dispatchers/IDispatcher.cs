using System.Threading;
using System.Threading.Tasks;
namespace KDR.Processors.Outgoing.Dispatchers
{
    public interface IDispatcher
    {
         ValueTask EnqueueToPublishAsync(object message);

         void EnqueueToPublish(object message);

         ValueTask<bool> WaitToReadQueuedToPublishAsync(CancellationToken cancellationToken);

         ValueTask<object> ReadQueuedToPublishAsync(CancellationToken cancellationToken);
    }
}