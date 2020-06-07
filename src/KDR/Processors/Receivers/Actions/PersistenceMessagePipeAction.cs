using System.Transactions;
using System;
using System.Threading.Tasks;
using KDR.Messages;
using KDR.Persistence;

namespace KDR.Processors.Receivers.Actions
{
  public class PersistenceMessagePipeAction : IReceivePipeAction
  {
    private readonly IDataStorage _dataStorage;

    public PersistenceMessagePipeAction(IDataStorage dataStorage)
    {
      _dataStorage = dataStorage;
    }

    public async Task ExecuteAsync(ReceivePipelineContext ctx, Func<Task> next)
    {
      if (!await _dataStorage.StoreReceivedMessageAsync(ctx.Load<Message>()))
      {
        //cancel
        return;
      }

      if(Transaction.Current != null)
      {
        var commitAction = ctx.Load<Func<Task>>(ReceivePipelineContext.CommitMessageAction);
        // Transaction.Current.TransactionCompleted += (sender, args) => 
        // {
        //   Transaction.Current.en
        //   if(args.Transaction.TransactionInformation.)
        //   commitAction()
        // }
      }

      await next();

      //coś jeszcze będzie potrzebne? Mógł nie mieć handlera
    }
  }
}
