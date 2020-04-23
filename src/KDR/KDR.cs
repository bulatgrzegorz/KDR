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

    }

    private static void InitializeCore(this IServiceCollection services)
    {
      if (_coreInitialized)
      {
        return;
      }

      RegisterSerializerModule(services);
      RegisterTransportModule(services);
    }

    private static void RegisterTransportModule(IServiceCollection services)
    {
      services.TryAddSingleton<IReceivePipeAction, PersistenceMessagePipeAction>();
      services.TryAddSingleton<IReceivePipeAction, TracePipeAction>();
      services.TryAddSingleton<IReceivePipeAction, TransactionPipeAction>();

      services.TryAddSingleton<IReceivePipeline>(
        sp => new ReceivePipeline(null)
          .AddAction(sp.GetRequiredService<TracePipeAction>())
          .AddAction(sp.GetRequiredService<SerializationPipeAction>())
          .AddAction(sp.GetRequiredService<TransactionPipeAction>())
          .AddAction(sp.GetRequiredService<PersistenceMessagePipeAction>())
          .AddAction(sp.GetRequiredService<HandlerInvokerPipeAction>())
          );

      services.TryAddSingleton<IPipelineInvoker, PipelineInvoker>();
    }

    private static void RegisterSerializerModule(IServiceCollection services)
    {
      services.TryAddSingleton<JsonSerializer>();
      services.TryAddSingleton<XmlSerializer>();

      services.TryAddSingleton<ISerializerFactory, SerializerFactory>();
    }
  }
}
