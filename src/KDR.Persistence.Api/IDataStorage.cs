using System.Collections.Generic;
using System.Threading.Tasks;

namespace KDR.Persistence.Api
{
  public interface IDataStorage
  {
    Task StoreMessageToSendAsync(DbMessage message);

    //Returns true if storing value finished successfully
    Task<bool> StoreReceivedMessageAsync(object body, IDictionary<string, string> headers);
  }
}
