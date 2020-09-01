using System;
using System.Threading.Tasks;
using System.Transactions;
using KDR.Persistence.Api;
using KDR.Processors.Outgoing.Dispatchers;

namespace KDR.Processors.Outgoing.Actions
{
    public class PersistenceMessagePipeAction : IOutgoingPipeAction
    {
        private readonly IDataStorage _dataStorage;
        private readonly IDispatcher _dispatcher;

        public PersistenceMessagePipeAction(IDataStorage dataStorage, IDispatcher dispatcher)
        {
            _dataStorage = dataStorage;
            _dispatcher = dispatcher;
        }

        public async Task ExecuteAsync(OutgoingPipelineContext ctx, Func<Task> next)
        {
            var dbMessage = ctx.Load<DbMessage>();

            await _dataStorage.StoreMessageToSendAsync(dbMessage);

            Action callDispatcher = () => _dispatcher.EnqueueToPublish(dbMessage);

            if(Transaction.Current != null)
            {
                Transactions.GenericTransaction.EnlistTransaction(
                    null,
                    callDispatcher,
                    null,
                    null
                );
            }
            else
            {
                callDispatcher();
            }
            
        }
    }
}