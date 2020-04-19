using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KDR.Processors
{
  public class ProcessorsManager : IProcessorsManager
  {
    private static readonly TimeSpan _taskAbortTimeout = TimeSpan.FromSeconds(5);
    private readonly ICollection<IProcessor> _processors;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private Task _task;
    private ProcessingContext _processingContext;
    private bool _disposed;

    public ProcessorsManager()
    {
      _processors = new List<IProcessor>();
      _cancellationTokenSource = new CancellationTokenSource();
    }

    public void AttachProcessor(IProcessor processor) => _processors.Add(processor);

    public Task StartAsync()
    {
      _processingContext = new ProcessingContext(_cancellationTokenSource.Token);

      var processorsTask = _processors.Select(x => x.ProcessAsync(_processingContext));
      _task = Task.WhenAll(processorsTask);

      return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
      if (_disposed)
      {
        return Task.CompletedTask;
      }

      try
      {
        _cancellationTokenSource.Dispose();
        _disposed = true;

        _task.Wait(_taskAbortTimeout);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      finally
      {
        //done
      }

      return Task.CompletedTask;
    }
  }
}
