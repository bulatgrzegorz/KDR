using System;
using System.Threading.Tasks;
using System.Transactions;

namespace KDR.Processors.Receivers.Actions
{
  public class TransactionPipeAction : IReceivePipeAction
  {
    public async Task ExecuteAsync(ReceivePipelineContext ctx, Func<Task> next)
    {
      using (var tran = new TransactionScope(
        TransactionScopeOption.Required,
        TransactionScopeAsyncFlowOption.Enabled))
      {
        await next();

        tran.Complete();
      }
    }
  }
}
