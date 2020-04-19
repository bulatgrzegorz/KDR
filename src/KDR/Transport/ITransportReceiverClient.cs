using System;
using System.Threading;
using System.Threading.Tasks;

namespace KDR.Transport
{
  public interface ITransportReceiverClient
  {
    //Sender, TransportMessage => Task
    Func<object, TransportMessage, Task> OnMessageReceive { get; set; }

    Task StartListeningAsync(CancellationToken cancellationToken);

    Task DisposeAsync(CancellationToken cancellationToken);

    Task CommitAsync(object sender, CancellationToken cancellationToken);
  }
}
