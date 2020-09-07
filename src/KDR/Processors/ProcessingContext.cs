using System.Threading;

namespace KDR.Processors
{
    public class ProcessingContext
    {
        public readonly CancellationToken CancellationToken;

        public ProcessingContext(CancellationToken cancellationToken)
        {
            CancellationToken = cancellationToken;
        }

        public bool Stopped => CancellationToken.IsCancellationRequested;
    }
}