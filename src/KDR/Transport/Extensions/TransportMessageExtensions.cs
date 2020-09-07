using KDR.Messages;
using KDR.Serialization;
using KDR.Transport.Api;

namespace KDR.Transport.Extensions
{
    public static class TransportMessageExtensions
    {
        public static string GetContentType(this TransportMessage tm)
        {
            if (tm.Headers.TryGetValue(MessageHeaders.ContentType, out var contentType))
            {
                return string.IsNullOrEmpty(contentType) ? ContentTypes.JsonContentType : contentType;
            }

            return ContentTypes.JsonContentType;
        }
    }
}