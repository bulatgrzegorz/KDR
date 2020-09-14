using System;
using System.Threading.Tasks;
using KDR.Messages;
using KDR.Serialization;
using KDR.Transport.Api;
using KDR.Utilities;

namespace KDR.Processors.Receivers.Actions
{
    public class SerializationPipeAction : IReceivePipeAction
    {
        private readonly ISerializerFactory _serializerFactory;

        public SerializationPipeAction(ISerializerFactory serializerFactory)
        {
            _serializerFactory = serializerFactory;
        }

        public async Task ExecuteAsync(ReceivePipelineContext ctx, Func<Task> next)
        {
            var transportMessage = ctx.Load<TransportMessage>();

            var serializer = _serializerFactory.Create(transportMessage);
            ctx.Save<Message>(await serializer.DeserializeAsync(transportMessage));

            await FuncInvoker.Invoke(next);
        }
    }
}