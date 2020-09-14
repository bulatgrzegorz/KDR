using System;
using System.Threading.Tasks;
using System.Transactions;
using KDR.Utilities;

namespace KDR.Processors.Receivers.Actions
{
    public class TransactionPipeAction : IReceivePipeAction
    {
        public async Task ExecuteAsync(ReceivePipelineContext ctx, Func<Task> next)
        {
            using(var tran = new TransactionScope(
                TransactionScopeOption.Required,
                TransactionScopeAsyncFlowOption.Enabled))
            {
                await FuncInvoker.Invoke(next);

                tran.Complete();
            }
        }
    }
}