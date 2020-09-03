using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KDR.Processors.Actions
{
    public class TracePipeActionBase<T> : IPipeAction<T>
    {
        public async Task ExecuteAsync(T ctx, Func<Task> next)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await next();

            stopwatch.Stop();
        }
    }
}