using System.Threading;
using System.Threading.Tasks;

namespace KDR.Transport.Api
{
    public interface ITransportSenderClient
    {
        Task<bool> SendAsync(TransportMessage transportMessage, CancellationToken cancellationToken);

        Task DisposeAsync(CancellationToken cancellationToken);
    }
}