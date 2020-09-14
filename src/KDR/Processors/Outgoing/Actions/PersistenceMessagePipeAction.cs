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
            var message = ctx.Load<Messages.Message>();

            var dbMessage = await _dataStorage.StoreMessageToSendAsync(message.Body, message.Headers);

            Action callDispatcher = () => _dispatcher.EnqueueToPublish(message, dbMessage.Id);

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