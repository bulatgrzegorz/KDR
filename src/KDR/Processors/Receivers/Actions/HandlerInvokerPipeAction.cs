using System;
using System.Threading.Tasks;
using KDR.Abstractions.Handlers;
using KDR.Abstractions.Messages;
using KDR.Messages;
using KDR.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace KDR.Processors.Receivers.Actions
{
    public class HandlerInvokerPipeAction : IReceivePipeAction
    {
        //TODO: może zamiast tego powinniśmy przekazywać IScopeCreator, który tworzy scope z service providerem
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

            //TODO: może ten scope powinien się na równi z transakcją dziać, nawet by pasowało
            using(var scope = _serviceProvider.CreateScope())
            {
                var handler = (IMessageHandler)scope.ServiceProvider.GetRequiredService(handlerType);

                await handler.HandleAsync((IMessage)ctx.Load<Message>().Body);
            }

            await FuncInvoker.Invoke(next);
        }
    }
}