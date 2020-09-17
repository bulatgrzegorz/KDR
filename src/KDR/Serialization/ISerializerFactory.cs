using KDR.Transport.Api;

namespace KDR.Serialization
{
    public interface ISerializerFactory
    {
        ISerializer Default { get; }

        ISerializer Create(TransportMessage transportMessage);

        ISerializer Create(string contentType);
    }
}