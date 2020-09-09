using System.Collections.Generic;
using System.Threading.Tasks;
using KDR.Abstractions.Messages;

namespace KDR
{
    //TODO: Można zastanowić się nad umożliwieniem publikowania obiektów nie implementujących interfejsu IEvent 
    //jeśli to wymagane, można wymusić nadanie CorrelationId w header albo jako parametr
    public interface IEventBus
    {
        Task PublishEventAsync(IEvent @event);

        Task PublishEventAsync(IEvent @event, IDictionary<string, string> additionalHeaders);
    }
}