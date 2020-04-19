using System.Threading;
using System.Threading.Tasks;

namespace KDR.Transport
{
  public interface ITransportSenderClient
  {
    Task SendAsync(TransportMessage transportMessage, CancellationToken cancellationToken);

    Task DisposeAsync(CancellationToken cancellationToken);
  }
}
