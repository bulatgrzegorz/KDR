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
            _receivePipelineInvoke = ctx => BuildPipelineActionsChainAction(ctx, receivePipeline.Actions.ToArray());
            _outgoingPipelineInvoke = ctx => BuildPipelineActionsChainAction(ctx, outgoingPipeline.Actions.ToArray());
        }

        private static Task BuildPipelineActionsChainAction<TContext>(TContext context, IPipeAction<TContext>[] pipeActions)
        {
            Task InvokerFunction(int index)
            {
                if (index == pipeActions.Length)
                {
                    return Task.CompletedTask;
                }

                Task InvokeNext() => InvokerFunction(index + 1);

                return pipeActions[index].ExecuteAsync(context, InvokeNext);
            }

            return InvokerFunction(0);
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
