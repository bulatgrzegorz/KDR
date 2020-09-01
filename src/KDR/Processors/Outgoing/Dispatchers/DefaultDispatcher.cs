 using System.Threading.Tasks;
using System.Threading.Channels;

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
    }
}
