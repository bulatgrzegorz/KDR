using System;
using KDR.Transport;
using KDR.Transport.Extensions;
using KDR.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace KDR.Serialization
{
  public class SerializerFactory : ISerializerFactory
  {
    private readonly IServiceProvider _serviceProvider;

    public SerializerFactory(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
    }

    public ISerializer Create(TransportMessage transportMessage)
    {
      var contentType = transportMessage.GetContentType();
      switch (transportMessage.GetContentType())
      {
        case ContentTypes.JsonContentType:
        case ContentTypes.JsonUtf8ContentType:
          return _serviceProvider.GetRequiredService<JsonSerializer>();

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
