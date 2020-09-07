using System;
using System.Threading.Tasks;

namespace KDR.Processors
{
    public class DelayedProcessor : IProcessor
    {
        private readonly TimeSpan _idle = TimeSpan.FromSeconds(5);
        private readonly IProcessor _next;

        public DelayedProcessor(IProcessor next)
        {
            _next = next;
        }

        public async Task<bool> ProcessAsync(ProcessingContext context)
        {
            try
            {
                if (!await _next.ProcessAsync(context).ConfigureAwait(false))
                {
                    try
                    {
                        await Task.Delay(_idle, context.CancellationToken);
                    }
                    catch (TaskCanceledException)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return true;
        }
    }
}