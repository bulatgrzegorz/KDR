using System.Collections.Generic;
using System.Threading.Tasks;

namespace KDR.Persistence.Api
{
    public interface IDataStorage
    {
        Task StoreMessageToSendAsync(DbMessage message);

        Task MarkMessageAsSendAsync(DbMessage message);

        Task MarkMessageAsFailedAsync(DbMessage message);

        //Returns id of stored message. TODO: When null failed? 
        Task<int?> StoreReceivedMessageAsync(ReceivedDbMessage message);

        Task < (IEnumerable<object> messages, bool gotMore) > GetMessagesToRetryAsync();
    }
}