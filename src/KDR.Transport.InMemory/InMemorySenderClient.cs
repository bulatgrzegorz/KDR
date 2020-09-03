using System.Threading;
using System.Threading.Tasks;
using KDR.Transport.Api;
using Microsoft.Extensions.Logging;

namespace KDR.Transport.InMemory
{
    public class InMemorySenderClient : ITransportSenderClient
    {
        private readonly ILogger<InMemorySenderClient> _logger;

        public InMemorySenderClient(ILogger<InMemorySenderClient> logger)
        {
            _logger = logger;
        }

        public Task DisposeAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SendAsync(TransportMessage transportMessage, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Got message to send!!!");

            MessagesStorage.Messages.Add(transportMessage);

            return Task.CompletedTask;
        }
    }
}