using System.Threading;
using System.Threading.Tasks;
using KDR.Persistence;

namespace KDR.Processors.Receivers
{
  public class DeduplicateMessageAction<TContext> : IReceivePipeAction<TContext> where TContext : IReceivePipeContext
  {
    private readonly IReceivePipeAction<TContext> _next;
    private readonly IDataStorage _dataStorage;

    public DeduplicateMessageAction(IReceivePipeAction<TContext> next, IDataStorage dataStorage)
    {
      _next = next;
      _dataStorage = dataStorage;
    }

    public async Task ExecuteAsync(TContext ctx, CancellationToken cancellationToken)
    {
      if (!await _dataStorage.MarkMessageAsSentAsync())
      {
        //cancel
        return;
      }

      await _next.ExecuteAsync(ctx, cancellationToken);

      //coś jeszcze będzie potrzebne?
    }
  }
}
