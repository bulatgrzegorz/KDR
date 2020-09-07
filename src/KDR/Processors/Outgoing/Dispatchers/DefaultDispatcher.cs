 using System.Threading.Channels;
 using System.Threading.Tasks;
 using System.Threading;

 namespace KDR.Processors.Outgoing.Dispatchers
 {
     public class DefaultDispatcher : IDispatcher
     {
         private readonly Channel<object> _outgoingChannel;

         public DefaultDispatcher()
         {
             _outgoingChannel = Channel.CreateUnbounded<object>(new UnboundedChannelOptions()
             {
                 SingleReader = true,
                 SingleWriter = true
             });
         }

         public void EnqueueToPublish(object message)
         {
             _outgoingChannel.Writer.TryWrite(message);
         }

         public ValueTask EnqueueToPublishAsync(object message)
         {
             return _outgoingChannel.Writer.WriteAsync(message);
         }

         public ValueTask<object> ReadQueuedToPublishAsync(CancellationToken cancellationToken)
         {
             return _outgoingChannel.Reader.ReadAsync(cancellationToken);
         }

         public ValueTask<bool> WaitToReadQueuedToPublishAsync(CancellationToken cancellationToken)
         {
             return _outgoingChannel.Reader.WaitToReadAsync(cancellationToken);
         }
     }
 }