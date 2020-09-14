using System;
using System.Threading.Tasks;
using KDR.Transport.Api;
using KDR.Utilities;

namespace KDR.Processors.Outgoing.Actions
{
    public class ProvideDestinationAddressPipeAction : IPipeAction<OutgoingPipelineContext>
    {
        private readonly IDestinationAddressProvider _destinationAddressProvider;

        public ProvideDestinationAddressPipeAction(IDestinationAddressProvider destinationAddressProvider)
        {
            _destinationAddressProvider = destinationAddressProvider;
        }

        public Task ExecuteAsync(OutgoingPipelineContext ctx, Func<Task> next)
        {
            var message = ctx.Load<Messages.Message>();

            message.Headers.Add(Messages.MessageHeaders.DestinationAddress,  _destinationAddressProvider.Get(message.Body.GetType()));

            return FuncInvoker.Invoke(next);
        }
    }
}