using System.Threading;
using System.Threading.Tasks;

namespace KDR.Processors.Receivers
{
  public interface IReceivePipeAction<in TContext> where TContext : IReceivePipeContext
  {
    Task ExecuteAsync(TContext ctx, CancellationToken cancellationToken);
  }
}
