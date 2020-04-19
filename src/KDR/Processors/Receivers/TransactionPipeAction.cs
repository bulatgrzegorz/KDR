using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace KDR.Processors.Receivers
{
  public class TransactionPipeAction<TContext> : IReceivePipeAction<TContext> where TContext : IReceivePipeContext
  {
    private readonly IReceivePipeAction<TContext> _next;

    public TransactionPipeAction(IReceivePipeAction<TContext> next)
    {
      _next = next;
    }

    public async Task ExecuteAsync(TContext ctx, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      using (var tran = new TransactionScope(
        TransactionScopeOption.Required,
        new TransactionOptions
        {
          IsolationLevel = IsolationLevel.ReadCommitted,
        },
        TransactionScopeAsyncFlowOption.Enabled))
      {
        await _next.ExecuteAsync(ctx, cancellationToken);

        tran.Complete();
      }
    }
  }
}
