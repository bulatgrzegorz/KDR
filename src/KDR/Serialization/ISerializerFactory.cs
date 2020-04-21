using KDR.Transport;

namespace KDR.Serialization
{
  public interface ISerializerFactory
  {
    ISerializer Create(TransportMessage transportMessage);
  }
}
