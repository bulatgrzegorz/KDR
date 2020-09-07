using System.Collections.Generic;
using System.Threading.Tasks;
using KDR.Abstractions.Messages;

namespace KDR
{
    public interface IEventBus
    {
        Task PublishEventAsync(IEvent @event);

        Task PublishEventAsync(IEvent @event, IDictionary<string, string> additionalHeaders);
    }
}