using System.Threading.Tasks;
using KDR.Messages;

namespace KDR.Persistence
{
  public interface IDataStorage
  {
    Task StoreMessageToSendAsync(DomainMessage message);
  }
}
