using System.Collections.Generic;
using System.Threading.Tasks;

namespace KDR.Persistence.Api
{
  public interface IDataStorage
  {
    Task StoreMessageToSendAsync(DbMessage message);

    //Returns id of stored message. TODO: When null failed? 
    Task<int?> StoreReceivedMessageAsync(ReceivedDbMessage message);
  }
}
