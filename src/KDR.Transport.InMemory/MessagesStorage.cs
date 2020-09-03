using System.Collections.Concurrent;
using KDR.Transport.Api;

namespace KDR.Transport.InMemory
{
    public static class MessagesStorage
    {
        public static ConcurrentBag<TransportMessage> Messages = new ConcurrentBag<TransportMessage>();
    }
}