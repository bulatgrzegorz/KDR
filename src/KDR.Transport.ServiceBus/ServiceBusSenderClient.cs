using System.IO;
using System.Threading;
using System.Threading.Tasks;
using KDR.Utilities;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace KDR.Transport.ServiceBus
{
  public class ServiceBusSenderClient : ITransportSenderClient
  {
    private readonly ServiceBusTransportOptions _options;
    private MessageSender _messageSender;

    public ServiceBusSenderClient(ServiceBusTransportOptions options)
    {
      Check.NotNull(options, nameof(ServiceBusTransportOptions));

      _options = options;
    }

    public async Task SendAsync(TransportMessage transportMessage, CancellationToken cancellationToken)
    {
      await ConnectAsync();

      //TODO: Wyciągnąć do jakichś converterów, tutaj zapewne będzie działo się zdecydowanie więcej
      var brokeredMessage = new BrokeredMessage(new MemoryStream(transportMessage.Body), ownsStream: true);
      foreach (var transportMessageHeader in transportMessage.Headers)
      {
        brokeredMessage.Properties[transportMessageHeader.Key] = transportMessageHeader.Value;
      }

      await _messageSender.SendAsync(brokeredMessage);
    }

    public Task DisposeAsync(CancellationToken cancellationToken)
    {
      return _messageSender?.CloseAsync();
    }

    private async Task ConnectAsync()
    {
      if (_messageSender != null)
      {
        return;
      }

      var factory = MessagingFactory.CreateFromConnectionString(_options.ConnectionString);
      _messageSender = await factory.CreateMessageSenderAsync(_options.EntityPath);
      _messageSender.RetryPolicy = RetryPolicy.Default;
    }
  }
}
