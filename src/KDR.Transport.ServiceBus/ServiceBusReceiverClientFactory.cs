using KDR.Transport.Factories;

namespace KDR.Transport.ServiceBus
{
  public class ServiceBusReceiverClientFactory : ITransportReceiverClientFactory
  {
    public ITransportReceiverClient Create(string entityPath)
    {
      //TODO: Zastanowić się jak ma wyglądać pobranie ServiceBusTransportOptions z kontenera (jeśli przez Options to czy nie powinno to być w innym projekcie?)
      return new ServiceBusReceiverClient(new ServiceBusTransportOptions("", entityPath, 1));
    }
  }
}
