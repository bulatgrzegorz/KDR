using System.Threading.Tasks;
using KDR.Messages;

namespace KDR.Persistence
{
  public interface IDataStorage
  {
    Task StoreMessageToSendAsync(DbMessage message);

    //Returns true if storing value finished successfully
    Task<bool> StoreReceivedMessageAsync(Message message);
  }
}
