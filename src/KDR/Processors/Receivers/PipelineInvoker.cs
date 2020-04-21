using System;
using System.Linq;
using System.Threading.Tasks;

namespace KDR.Processors.Receivers
{
  public class PipelineInvoker //: IPipelineInvoker
  {
    private readonly Func<ReceivePipeActionContext, Task> _pipelineInvoke;

    public PipelineInvoker(IReceivePipeline pipeline)
    {
      var pipelineActions = pipeline.Actions.ToArray();

      Task ProcessIncoming(ReceivePipeActionContext context)
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

    public Task InvokeAsync()
    {
      return _pipelineInvoke(new ReceivePipeActionContext());
    }
  }
}
