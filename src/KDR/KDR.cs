using System;
using System.Collections.Generic;
using KDR.Persistence.Api;
using KDR.Processors.Dispatchers;
using KDR.Processors.Outgoing;
using KDR.Processors.Outgoing.Dispatchers;
using KDR.Processors.Receivers;
using KDR.Processors.Receivers.Actions;
using KDR.Serialization;
using KDR.Transport;
using KDR.Transport.Api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KDR
{
    public class KDR
    {
        private static readonly bool _coreInitialized = false;
        private static IDictionary<Type, string> _destinationMap;

        public static void CreateEventBus(IServiceCollection services)
        {
            InitializeCore(services);
        }

        public static void UseDefaultDestinationAddressProvider(IServiceCollection services)
        {
            services.AddSingleton<IDestinationAddressProvider>(new DefaultDestinationAddressProvider(_destinationMap));
        }

        public static void MapDestination<TMessage>(string dest)
        {
            _destinationMap = _destinationMap ?? new Dictionary<Type, string>();

            _destinationMap.Add(typeof(TMessage), dest);
        }

        private static void InitializeCore(IServiceCollection services)
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
            services.TryAddSingleton<InMemorySendingDispatcher>();
            services.TryAddSingleton<Processors.Outgoing.Actions.PersistenceMessagePipeAction>();
            services.TryAddSingleton<Processors.Outgoing.Actions.PrepareMessageMetadataPipeAction>();
            services.TryAddSingleton<Processors.Outgoing.Actions.TracePipeAction>();
            services.TryAddSingleton<Processors.Outgoing.Actions.ProvideDestinationAddressPipeAction>();

            services.TryAddSingleton<IOutgoingPipeline>(
                sp => new OutgoingPipeline(null)
                .AddAction(sp.GetRequiredService<Processors.Outgoing.Actions.TracePipeAction>())
                .AddAction(sp.GetRequiredService<Processors.Outgoing.Actions.ProvideDestinationAddressPipeAction>())
                .AddAction(sp.GetRequiredService<Processors.Outgoing.Actions.PrepareMessageMetadataPipeAction>())
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