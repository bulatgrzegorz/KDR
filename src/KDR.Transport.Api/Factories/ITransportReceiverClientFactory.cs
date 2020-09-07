namespace KDR.Transport.Api.Factories
{
    public interface ITransportReceiverClientFactory
    {
        ITransportReceiverClient Create(string entityPath);
    }
}