using System;
using KDR.Transport.Api;
using KDR.Transport.Extensions;
using KDR.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace KDR.Serialization
{
    public class SerializerFactory : ISerializerFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ISerializer _jsonSerializer;

        public SerializerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _jsonSerializer = _serviceProvider.GetRequiredService<JsonSerializer>();
        }

        public ISerializer Default => _jsonSerializer;

        public ISerializer Create(TransportMessage transportMessage)
        {
            var contentType = transportMessage.GetContentType();
            return Create(transportMessage.GetContentType());
        }

        public ISerializer Create(string contentType)
        {
            switch (contentType)
            {
                case ContentTypes.JsonContentType:
                case ContentTypes.JsonUtf8ContentType:
                    return _jsonSerializer;

                case ContentTypes.TextContentType:
                case ContentTypes.XmlContentType:
                    return _serviceProvider.GetRequiredService<XmlSerializer>();

                default:
                    //TODO: [LogTransportMessage]
                    var supportedContentTypes = typeof(ContentTypes).GetAllPublicConstantValues<string>();
                    throw new NotSupportedException(
                        $"Content type: {contentType} has no supported serializer. Supported are: {string.Join(",", supportedContentTypes)}");
            }
        }
    }
}