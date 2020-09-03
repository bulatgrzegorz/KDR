using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KDR.Processors.Dispatchers;
using Microsoft.Extensions.DependencyInjection;

namespace KDR.Processors
{
    public class ProcessorsManager : IProcessorsManager
    {
        private static readonly TimeSpan _taskAbortTimeout = TimeSpan.FromSeconds(5);
        private readonly CancellationTokenSource _cancellationTokenSource;
        private Task _task;
        private ProcessingContext _processingContext;
        private bool _disposed;

        private readonly IServiceProvider _serviceProvider;

        public ProcessorsManager(IServiceProvider serviceProvider)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync()
        {
            _processingContext = new ProcessingContext(_cancellationTokenSource.Token);

            var processors = GetProcessors(_serviceProvider)
            .Select(x => new FaultTolerantInfiniteProcessor(x))
            .Select(x => x.ProcessAsync(_processingContext));
            _task = Task.WhenAll(processors);

            return Task.CompletedTask;
        }

        private IEnumerable<IProcessor> GetProcessors(IServiceProvider sp)
        {
            yield return sp.GetRequiredService<InMemorySendingDispatcher>();
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
