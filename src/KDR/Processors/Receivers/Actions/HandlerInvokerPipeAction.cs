using System;
using System.Threading.Tasks;
using KDR.Abstractions.Handlers;
using KDR.Abstractions.Messages;
using KDR.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace KDR.Processors.Receivers.Actions
{
    public class HandlerInvokerPipeAction : IReceivePipeAction
    {
        private readonly IServiceProvider _serviceProvider;

        public HandlerInvokerPipeAction(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task ExecuteAsync(ReceivePipelineContext ctx, Func<Task> next)
        {
            var message = ctx.Load<Message>();
            var messageType = message.Headers[MessageHeaders.EventType];

            var handlerType = MessageHandlersMapper.GetHandler(messageType);

            var handler = (IMessageHandler)_serviceProvider.GetRequiredService(handlerType);

            await handler.HandleAsync((IMessage)ctx.Load<Message>().Body);

            await next();
        }
    }
}