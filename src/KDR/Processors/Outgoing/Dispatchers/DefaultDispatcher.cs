 using System.Threading.Channels;
 using System.Threading.Tasks;
 using System.Threading;
using KDR.Messages;
using System;

namespace KDR.Processors.Outgoing.Dispatchers
 {
     public class DefaultDispatcher : IDispatcher
     {
         private readonly Channel<(Message message, Guid dbMessageId)> _outgoingChannel;

         public DefaultDispatcher()
         {
             _outgoingChannel = Channel.CreateUnbounded<(Message message, Guid dbMessageId)>(new UnboundedChannelOptions()
             {
                 SingleReader = true,
                 SingleWriter = true
             });
         }

        public void EnqueueToPublish(Message message, Guid dbMessageId)
        {
            _outgoingChannel.Writer.TryWrite((message, dbMessageId));
        }

        public ValueTask EnqueueToPublishAsync(Message message, Guid dbMessageId)
        {
            return _outgoingChannel.Writer.WriteAsync((message, dbMessageId));
        }

         public ValueTask<bool> WaitToReadQueuedToPublishAsync(CancellationToken cancellationToken)
         {
             return _outgoingChannel.Reader.WaitToReadAsync(cancellationToken);
         }

        public ValueTask<(Message message, Guid dbMessageId)> ReadQueuedToPublishAsync(CancellationToken cancellationToken)
        {
            return _outgoingChannel.Reader.ReadAsync(cancellationToken);
        }
    }
 }