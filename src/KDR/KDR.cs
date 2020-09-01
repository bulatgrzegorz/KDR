using System;
using KDR.Persistence.Api;
using KDR.Processors.Outgoing;
using KDR.Processors.Outgoing.Dispatchers;
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
            InitializeCore(services);

        }

        private static void InitializeCore(this IServiceCollection services)
        {
            if (_coreInitialized)
            {
                return;
            }

            RegisterSerializerModule(services);
            RegisterTransportModule(services);

            RegisterDataStorage(services);

            services.TryAddSingleton<IEventBus, EventBus>();
        }

        private static void RegisterDataStorage(IServiceCollection services)
        {
            // // services.TryAddSingleton<IDataStorage, InMemoryDataStorage>();
        }

        private static void RegisterTransportModule(IServiceCollection services)
        {
            services.TryAddSingleton<PersistenceMessagePipeAction>();
            services.TryAddSingleton<TracePipeAction>();
            services.TryAddSingleton<TransactionPipeAction>();
            services.TryAddSingleton<SerializationPipeAction>();
            services.TryAddSingleton<HandlerInvokerPipeAction>();

            services.TryAddSingleton<IReceivePipeline>(
              sp => new ReceivePipeline(null)
                .AddAction(sp.GetRequiredService<TracePipeAction>())
                .AddAction(sp.GetRequiredService<SerializationPipeAction>())
                .AddAction(sp.GetRequiredService<TransactionPipeAction>())
                .AddAction(sp.GetRequiredService<PersistenceMessagePipeAction>())
                .AddAction(sp.GetRequiredService<HandlerInvokerPipeAction>())
                );

            services.TryAddSingleton<IPipelineInvoker, PipelineInvoker>();


            services.TryAddSingleton<IDispatcher, DefaultDispatcher>();
            services.TryAddSingleton<Processors.Outgoing.Actions.PersistenceMessagePipeAction>();
            services.TryAddSingleton<Processors.Outgoing.Actions.SerializationPipeAction>();

            services.TryAddSingleton<IOutgoingPipeline>(
              sp => new OutgoingPipeline(null)
                .AddAction(sp.GetRequiredService<Processors.Outgoing.Actions.SerializationPipeAction>())
                .AddAction(sp.GetRequiredService<Processors.Outgoing.Actions.PersistenceMessagePipeAction>())
                 );
        }

        private static void RegisterSerializerModule(IServiceCollection services)
        {
            services.TryAddSingleton<JsonSerializer>();
            services.TryAddSingleton<XmlSerializer>();

            services.TryAddSingleton<ISerializerFactory, SerializerFactory>();
        }
    }
}
