using System;
using System.Threading.Tasks;
using System.Transactions;

namespace KDR.Processors.Receivers.Actions
{
  public class TransactionPipeAction : IReceivePipeAction
  {
    public async Task ExecuteAsync(ReceivePipeActionContext ctx, Func<Task> next)
    {
      using (var tran = new TransactionScope(
        TransactionScopeOption.Required,
        new TransactionOptions
        {
          IsolationLevel = IsolationLevel.ReadCommitted,
        },
        TransactionScopeAsyncFlowOption.Enabled))
      {
        await next();

        tran.Complete();
      }
    }
  }
}
