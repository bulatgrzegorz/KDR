using System.Collections.Generic;

namespace KDR.Transport
{
    public class Message
    {
        public IDictionary<string, string> Headers { get; }

        public object Value { get; }
    }
}