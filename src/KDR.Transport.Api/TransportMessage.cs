using System.Collections.Generic;

namespace KDR.Transport.Api
{
    public class TransportMessage
    {
        public TransportMessage(IDictionary<string, string> headers, byte[] body)
        {
            Headers = headers;
            Body = body;
        }

        public IDictionary<string, string> Headers { get; }

        public byte[] Body { get; }
    }
}