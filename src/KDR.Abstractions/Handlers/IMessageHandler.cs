using System.Threading.Tasks;
using KDR.Abstractions.Messages;

namespace KDR.Abstractions.Handlers
{
  public interface IMessageHandler
  {
    Task HandleAsync(IMessage message);
  }
}
