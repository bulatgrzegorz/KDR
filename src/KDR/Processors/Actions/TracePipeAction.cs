using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace KDR.Processors.Actions
{
    public class TracePipeActionBase<T> : IPipeAction<T>
    {
        private readonly ILogger<TracePipeActionBase<T>> _logger;

        public TracePipeActionBase(ILogger<TracePipeActionBase<T>> logger)
        {
            _logger = logger;
        }

        public async Task ExecuteAsync(T ctx, Func<Task> next)
        {
            var operationId = Guid.NewGuid().ToString();
            _logger.LogInformation($"[{operationId}] Starting to publish event {ctx.ToString()}...");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                await next();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"[{operationId}] Error occured while publishing event {ctx.ToString()}");
            }
            
            stopwatch.Stop();

            _logger.LogInformation($"[{operationId}] Publishing event successfully ended. Took: {stopwatch.ElapsedMilliseconds.ToString()}");
        }
    }
}