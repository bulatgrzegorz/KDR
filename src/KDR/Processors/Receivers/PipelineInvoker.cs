using System;
using System.Linq;
using System.Threading.Tasks;
using KDR.Processors.Outgoing;
using KDR.Transport;
using KDR.Transport.Api;

namespace KDR.Processors.Receivers
{
    //TODO: Trzeba posprzątać
    public class PipelineInvoker : IPipelineInvoker
    {
        private readonly Func<ReceivePipelineContext, Task> _receivePipelineInvoke;
        private readonly Func<OutgoingPipelineContext, Task> _outgoingPipelineInvoke;

        public PipelineInvoker(IReceivePipeline receivePipeline, IOutgoingPipeline outgoingPipeline)
        {
            var receivePipelineActions = receivePipeline.Actions.ToArray();

            Task ProcessIncoming(ReceivePipelineContext context)
            {
                Task InvokerFunction(int index)
                {
                    if (index == receivePipelineActions.Length)
                    {
                        return Task.CompletedTask;
                    }

                    Task InvokeNext() => InvokerFunction(index + 1);

                    return receivePipelineActions[index].ExecuteAsync(context, InvokeNext);
                }

                return InvokerFunction(0);
            }

            _receivePipelineInvoke = ProcessIncoming;

            var outgoingPipelineActions = outgoingPipeline.Actions.ToArray();

            Task ProcessIncomingO(OutgoingPipelineContext context)
            {
                Task InvokerFunction(int index)
                {
                    if (index == receivePipelineActions.Length)
                    {
                        return Task.CompletedTask;
                    }

                    Task InvokeNext() => InvokerFunction(index + 1);

                    return outgoingPipelineActions[index].ExecuteAsync(context, InvokeNext);
                }

                return InvokerFunction(0);
            }

            _outgoingPipelineInvoke = ProcessIncomingO;
        }

        public Task InvokeAsync(ReceivePipelineContext context)
        {
            return _receivePipelineInvoke(context);
        }

        public Task InvokeAsync(TransportMessage message)
        {
            var context = new ReceivePipelineContext();
            context.Save<TransportMessage>(message);

            return _receivePipelineInvoke(context);
        }

        public Task InvokeAsync(OutgoingPipelineContext context)
        {
            return _outgoingPipelineInvoke(context);
        }
    }
}
