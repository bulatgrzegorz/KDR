using System.Threading.Tasks;

namespace KDR.Persistence
{
  public interface IDataStorage
  {
    Task StoreMessageToSendAsync(DbMessage message);

    Task<bool> MarkMessageAsSentAsync();
  }
}
