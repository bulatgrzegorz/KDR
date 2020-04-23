using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KDR.Processors.Receivers.Actions
{
  public class TracePipeAction : IReceivePipeAction
  {
    public async Task ExecuteAsync(ReceivePipelineContext ctx, Func<Task> next)
    {
      var stopwatch = new Stopwatch();
      stopwatch.Start();

      await next();

      stopwatch.Stop();
    }
  }
}
