using System;
using System.Threading.Tasks;
using KDR.Messages;
using KDR.Utilities;

namespace KDR.Processors.Outgoing.Actions
{
    public class PrepareMessageMetadataPipeAction : IOutgoingPipeAction
    {
        public Task ExecuteAsync(OutgoingPipelineContext ctx, Func<Task> next)
        {
            var message = ctx.Load<Messages.Message>();

            message.Headers[MessageHeaders.EventType] = MessageTypeConverters.GetTypeName(message.Body.GetType());

            return FuncInvoker.Invoke(next);
        }
    }
}