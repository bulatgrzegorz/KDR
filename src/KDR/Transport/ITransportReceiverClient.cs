using System;
using System.Threading;
using System.Threading.Tasks;

namespace KDR.Transport
{
  public interface ITransportReceiverClient
  {
    event EventHandler<TransportMessage> OnMessageReceived;

    Task StartListeningAsync(CancellationToken cancellationToken);

    Task DisposeAsync(CancellationToken cancellationToken);

    Task CommitAsync(object sender, CancellationToken cancellationToken);
  }
}
