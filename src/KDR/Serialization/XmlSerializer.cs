using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using KDR.Messages;
using KDR.Transport.Api;

namespace KDR.Serialization
{
    public class XmlSerializer : ISerializer
    {
        private static readonly Encoding DefaultEncoding = Encoding.UTF8;

        public ValueTask<Message> DeserializeAsync(TransportMessage transportMessage)
        {
            return new ValueTask<Message>(Deserialize(transportMessage));
        }

        public ValueTask<TransportMessage> SerializeAsync(Message message)
        {
            return new ValueTask<TransportMessage>(Serialize(message));
        }

        private TransportMessage Serialize(Message message)
        {
            var dataContractSerializer = new System.Runtime.Serialization.DataContractSerializer(message.Body.GetType());

            var headers = new Dictionary<string, string>(message.Headers)
                {
                    [MessageHeaders.ContentType] = ContentTypes.JsonUtf8ContentType, 
                    [MessageHeaders.EventType] = MessageTypeConverters.GetTypeName(message.Body.GetType())
                };

            using(var memoryStream = new MemoryStream())
            {
                dataContractSerializer.WriteObject(memoryStream, message.Body);

                return new TransportMessage(headers, memoryStream.ToArray());
            }
        }

        private Message Deserialize(TransportMessage message)
        {
            var messageTypeName = message.Headers[MessageHeaders.EventType];
            var messageType = MessageTypeConverters.GetNameType(messageTypeName);

            var dataContractSerializer = new System.Runtime.Serialization.DataContractSerializer(messageType);

            using(var memoryStream = new MemoryStream(message.Body))
            {
                return new Message(dataContractSerializer.ReadObject(memoryStream), new Dictionary<string, string>(message.Headers));
            }
        }
    }
}