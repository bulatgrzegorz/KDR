using KDR.Processors.Receivers;
using KDR.Processors.Receivers.Actions;
using KDR.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KDR
{
  public static class KDR
  {
    private static readonly bool _coreInitialized = false;

    public static void CreateEventBus(this IServiceCollection services)
    {
      var
    }

    private static void InitializeCore(this IServiceCollection services)
    {
      if (_coreInitialized)
      {
        return;
      }

      RegisterSerializerModule(services);
    }

    private static void RegisterTransportModule(IServiceCollection services)
    {
      services.TryAddSingleton<IReceivePipeAction, PersistenceMessagePipeAction>();
      services.TryAddSingleton<IReceivePipeAction, TracePipeAction>();
      services.TryAddSingleton<IReceivePipeAction, TransactionPipeAction>();

      services.TryAddSingleton<IReceivePipeline>(
        sp => new ReceivePipeline(null)
          .AddAction(sp.GetRequiredService<TracePipeAction>())
          .AddAction(sp.GetRequiredService<TransactionPipeAction>())
          .AddAction(sp.GetRequiredService<PersistenceMessagePipeAction>()));
    }

    private static void RegisterSerializerModule(IServiceCollection services)
    {
      services.TryAddSingleton<JsonSerializer>();
      services.TryAddSingleton<XmlSerializer>();

      services.TryAddSingleton<ISerializerFactory, SerializerFactory>();
    }
  }
}
