using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace KDR.Processors
{
    public class FaultTolerantInfiniteProcessor : IProcessor
    {
        private readonly IProcessor _next;
        private readonly ILogger<FaultTolerantInfiniteProcessor> _logger;
        private readonly TimeSpan _idleAfterError = TimeSpan.FromSeconds(1);

        public FaultTolerantInfiniteProcessor(IProcessor next, ILogger<FaultTolerantInfiniteProcessor> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task<bool> ProcessAsync(ProcessingContext context)
        {
            while (!context.Stopped)
            {
                try
                {
                    await _next.ProcessAsync(context);
                }
                catch (OperationCanceledException) { }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    try
                    {
                        await Task.Delay(_idleAfterError, context.CancellationToken);
                    }
                    catch (OperationCanceledException) { }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }
                }
            }

            _logger.LogInformation("Ending processing...");
            return false;
        }
    }
}