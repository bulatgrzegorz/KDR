using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using KDR.Messages;
using KDR.Transport.Api;
using Newtonsoft.Json;

namespace KDR.Serialization
{
    public class JsonSerializer : ISerializer
    {
        private static readonly Encoding DefaultEncoding = Encoding.UTF8;

        private static readonly JsonSerializerSettings DefaultSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All
        };

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
            var body = JsonConvert.SerializeObject(message.Body, DefaultSettings);
            var bodyBytes = DefaultEncoding.GetBytes(body);

            //TODO: Powinno zostać wyciągnięte
            var headers = new Dictionary<string, string>(message.Headers)
                {
                    [MessageHeaders.ContentType] = ContentTypes.JsonUtf8ContentType, [MessageHeaders.EventType] = MessageTypeConverters.GetTypeName(message.Body.GetType())
                };

            return new TransportMessage(headers, bodyBytes);
        }

        private Message Deserialize(TransportMessage message)
        {
            var messageTypeName = message.Headers[MessageHeaders.EventType];
            var messageType = MessageTypeConverters.GetNameType(messageTypeName);

            var bodyString = DefaultEncoding.GetString(message.Body);

            return new Message(JsonConvert.DeserializeObject(bodyString, messageType), new Dictionary<string, string>(message.Headers));
        }

        public ValueTask<(string serializedBody, string serializedHeaders)> SerializeAsync(object body, IDictionary<string, string> headers = null)
        {
            var serializedBody = JsonConvert.SerializeObject(body, DefaultSettings);
            if (headers == null)
            {
                headers = new Dictionary<string, string>();
            }

            headers[MessageHeaders.ContentType] = ContentTypes.JsonUtf8ContentType;

            var serializedHeaders = JsonConvert.SerializeObject(headers, DefaultSettings);

            return new ValueTask<(string serializedBody, string serializedHeaders)>((serializedBody, serializedHeaders));
        }
    }
}