using System;
using System.Threading.Tasks;

namespace KDR.Processors
{
  public class FaultTolerantInfiniteProcessor : IProcessor
  {
    private readonly IProcessor _next;
    private readonly TimeSpan _idleAfterError = TimeSpan.FromSeconds(1);

    public FaultTolerantInfiniteProcessor(IProcessor next)
    {
      _next = next;
    }

    public async Task<bool> ProcessAsync(ProcessingContext context)
    {
      while (!context.Stopped)
      {
        try
        {
          await _next.ProcessAsync(context);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
          try
          {
            await Task.Delay(_idleAfterError, context.CancellationToken);
          }
          catch (OperationCanceledException)
          {
          }
          catch (Exception exception)
          {
            Console.WriteLine(exception);
          }
        }
      }

      return false;
    }
  }
}
