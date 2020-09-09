using System.Collections.Generic;

namespace KDR.Messages
{
    public class Message
    {
        public Message(object body, IDictionary<string, string> headers = null)
        {
            Body = body;
            Headers = headers ?? new Dictionary<string, string>();
        }

        public object Body { get; }

        public IDictionary<string, string> Headers { get; }
    }
}