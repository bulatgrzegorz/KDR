using System.Collections.Generic;
using System.Threading.Tasks;
using KDR.Messages;
using KDR.Transport.Api;

namespace KDR.Serialization
{
    public interface ISerializer
    {
        ValueTask<Message> DeserializeAsync(TransportMessage transportMessage);

        ValueTask<TransportMessage> SerializeAsync(Message message);

        ValueTask<(string serializedBody, string serializedHeaders)> SerializeAsync(object body, IDictionary<string, string> headers = null);
    }
}