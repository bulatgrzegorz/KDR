using System.Threading.Tasks;
using KDR.Messages;
using KDR.Transport;

namespace KDR.Serialization
{
  public interface ISerializer
  {
    ValueTask<Message> DeserializeAsync(TransportMessage transportMessage);

    ValueTask<TransportMessage> SerializeAsync(Message message);
  }
}
