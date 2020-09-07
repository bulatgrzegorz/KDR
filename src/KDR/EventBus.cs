using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using KDR.Abstractions.Messages;
using KDR.Processors.Outgoing;
using KDR.Processors.Receivers;

namespace KDR
{
    public class EventBus : IEventBus
    {
        private readonly IPipelineInvoker _pielineInvoker;

        public EventBus(IPipelineInvoker pielineInvoker)
        {
            _pielineInvoker = pielineInvoker;
        }

        public async Task PublishEventAsync(IEvent @event)
        {
            var eventType = @event.GetType();

            var ctx = new OutgoingPipelineContext();
            ctx.Save(@event);
            await _pielineInvoker.InvokeAsync(ctx);
        }

        public Task PublishEventAsync(IEvent @event, IDictionary<string, string> additionalHeaders)
        {
            throw new System.NotImplementedException();
        }
    }
}