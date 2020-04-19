namespace KDR.Transport.Factories
{
  public interface ITransportReceiverClientFactory
  {
    ITransportReceiverClient Create(string entityPath);
  }
}
