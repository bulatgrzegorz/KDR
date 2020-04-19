using KDR.Utilities;

namespace KDR.Transport.ServiceBus
{
  public class ServiceBusTransportOptions
  {
    public ServiceBusTransportOptions(string connectionString, string entityPath, int maxConcurrentDispatcherCalls)
    {
      Check.NotEmpty(entityPath, nameof(EntityPath));
      Check.NotEmpty(connectionString, nameof(ConnectionString));

      ConnectionString = connectionString;
      EntityPath = entityPath;
      MaxConcurrentDispatcherCalls = maxConcurrentDispatcherCalls;
    }

    public string ConnectionString { get; }

    public string EntityPath { get; }

    public int MaxConcurrentDispatcherCalls { get; }
  }
}
