using KDR.Transport.Api;

namespace KDR.Serialization
{
  public interface ISerializerFactory
  {
    ISerializer Create(TransportMessage transportMessage);

    ISerializer Create(string contentType);
  }
}
