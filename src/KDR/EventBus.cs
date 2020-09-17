using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using KDR.Abstractions.Messages;
using KDR.Processors.Outgoing;
using KDR.Processors.Receivers;
using KDR.Messages;

namespace KDR
{
    public class EventBus : IEventBus
    {
        private readonly IPipelineInvoker _pielineInvoker;

        public EventBus(IPipelineInvoker pielineInvoker)
        {
            _pielineInvoker = pielineInvoker;
        }

        public Task PublishEventAsync(IEvent @event)
        {
            var message = new Message(@event, new Dictionary<string, string>());

            var ctx = new OutgoingPipelineContext();
            ctx.Save(message);

            return _pielineInvoker.InvokeAsync(ctx);
        }

        public Task PublishEventAsync(IEvent @event, IDictionary<string, string> additionalHeaders)
        {
            var message = new Message(@event, additionalHeaders);
            
            var ctx = new OutgoingPipelineContext();
            ctx.Save(message);

            return _pielineInvoker.InvokeAsync(ctx);
        }
    }
}