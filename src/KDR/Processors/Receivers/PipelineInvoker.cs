using System;
using System.Linq;
using System.Threading.Tasks;
using KDR.Transport;

namespace KDR.Processors.Receivers
{
  public class PipelineInvoker : IPipelineInvoker
  {
    private readonly Func<ReceivePipelineContext, Task> _pipelineInvoke;

    public PipelineInvoker(IReceivePipeline pipeline)
    {
      var pipelineActions = pipeline.Actions.ToArray();

      Task ProcessIncoming(ReceivePipelineContext context)
      {
        Task InvokerFunction(int index)
        {
          if (index == pipelineActions.Length)
          {
            return Task.CompletedTask;
          }

          Task InvokeNext() => InvokerFunction(index + 1);

          return pipelineActions[index].ExecuteAsync(context, InvokeNext);
        }

        return InvokerFunction(0);
      }

      _pipelineInvoke = ProcessIncoming;
    }

    public Task InvokeAsync(ReceivePipelineContext context)
    {
      return _pipelineInvoke(context);
    }

    public Task InvokeAsync(TransportMessage message)
    {
      var context = new ReceivePipelineContext();
      context.Save<TransportMessage>(message);
      
      return _pipelineInvoke(context);
    }
  }
}
