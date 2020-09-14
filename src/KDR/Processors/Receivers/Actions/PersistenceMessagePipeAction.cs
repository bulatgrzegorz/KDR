using System;
using System.Threading.Tasks;
using System.Transactions;
using KDR.Messages;
using KDR.Persistence.Api;
using KDR.Utilities;

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
            var message = ctx.Load<Message>();
            var messageId = await _dataStorage.StoreReceivedMessageAsync(new Persistence.ReceivedDbMessage()
            {
                Body = message.Body,
                Headers = message.Headers
            });
            if (messageId == null)
            {
                //TODO: Canceled vote in context?
                return;
            }

            //TODO: co tutaj robi transakcja? bo powinno się kończyć na samym dole
            if (Transaction.Current != null)
            {
                var commitAction = ctx.Load<Func<Task>>(ReceivePipelineContext.CommitMessageAction);
                // Transaction.Current.TransactionCompleted += (sender, args) => 
                // {
                //   Transaction.Current.en
                //   if(args.Transaction.TransactionInformation.)
                //   commitAction()
                // }
            }

            await FuncInvoker.Invoke(next);

            //coś jeszcze będzie potrzebne? Mógł nie mieć handlera
        }
    }
}