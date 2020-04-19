using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace KDR.Processors.Receivers
{
  public class TracePipeAction<TContext> : IReceivePipeAction<TContext> where TContext : IReceivePipeContext
  {
    private readonly IReceivePipeAction<TContext> _next;

    public TracePipeAction(IReceivePipeAction<TContext> next)
    {
      _next = next;
    }

    public async Task ExecuteAsync(TContext ctx, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var stopwatch = new Stopwatch();
      stopwatch.Start();

      await _next.ExecuteAsync(ctx, cancellationToken);

      stopwatch.Stop();
    }
  }
}
