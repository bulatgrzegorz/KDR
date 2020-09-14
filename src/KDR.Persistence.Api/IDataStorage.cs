using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KDR.Persistence.Api
{
    public interface IDataStorage
    {
        /// <summary>
        /// Store message to be send.
        /// </summary>
        /// <param name="Body">Body of message to send</param>
        /// <param name="headers"Headers of message></param>
        /// <returns>Database </returns>
        Task<DbMessage> StoreMessageToSendAsync(object Body, IDictionary<string, string> headers);

        Task MarkMessageAsSendAsync(Guid messageId);

        Task MarkMessageAsFailedAsync(Guid messageId);

        //Returns id of stored message. TODO: When null failed? 
        Task<int?> StoreReceivedMessageAsync(ReceivedDbMessage message);

        Task < (IEnumerable<DbMessage> messages, bool gotMore) > GetMessagesToRetryAsync();
    }
}