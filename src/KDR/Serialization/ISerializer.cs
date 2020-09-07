using System.Threading.Tasks;
using KDR.Messages;
using KDR.Transport.Api;

namespace KDR.Serialization
{
    public interface ISerializer
    {
        ValueTask<Message> DeserializeAsync(TransportMessage transportMessage);

        ValueTask<TransportMessage> SerializeAsync(Message message);
    }
}