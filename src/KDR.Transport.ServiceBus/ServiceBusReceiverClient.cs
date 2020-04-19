using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KDR.Utilities;
using Microsoft.ServiceBus.Messaging;

namespace KDR.Transport.ServiceBus
{
  public class ServiceBusReceiverClient : ITransportReceiverClient
  {
    private readonly ServiceBusTransportOptions _options;
    private MessageReceiver _messageReceiver;

    public ServiceBusReceiverClient(ServiceBusTransportOptions options)
    {
      Check.NotNull(options, nameof(ServiceBusTransportOptions));
      Check.NotEmpty(options.EntityPath, nameof(ServiceBusTransportOptions.EntityPath));
      Check.NotEmpty(options.ConnectionString, nameof(ServiceBusTransportOptions.ConnectionString));

      _options = options;
    }

    public Func<object, TransportMessage, Task> OnMessageReceive { get; set; }

    public async Task StartListeningAsync(CancellationToken cancellationToken)
    {
      Check.NotNull(OnMessageReceive, nameof(OnMessageReceive));

      await ConnectAsync();

      _messageReceiver.OnMessageAsync(
        OnMessageReceivedAsync,
        new OnMessageOptions()
        {
          AutoComplete = false,
          MaxConcurrentCalls = _options.MaxConcurrentDispatcherCalls
        });
    }

    public Task DisposeAsync(CancellationToken cancellationToken)
    {
      return _messageReceiver?.CloseAsync();
    }

    public Task CommitAsync(object sender, CancellationToken cancellationToken)
    {
      if (!Guid.TryParse(sender?.ToString(), out var senderGuid))
      {
        throw new ArgumentException();
      }

      return _messageReceiver.CompleteAsync(senderGuid);
    }

    private static byte[] ReadFully(Stream input)
    {
      using (var ms = new MemoryStream())
      {
        input.CopyTo(ms);
        return ms.ToArray();
      }
    }

    private async Task ConnectAsync()
    {
      if (_messageReceiver != null)
      {
        return;
      }

      var messagingFactory = MessagingFactory.CreateFromConnectionString(_options.ConnectionString);
      _messageReceiver = await messagingFactory.CreateMessageReceiverAsync(_options.EntityPath, ReceiveMode.PeekLock);
    }

    private async Task OnMessageReceivedAsync(BrokeredMessage brokeredMessage)
    {
      Check.NotNull(brokeredMessage, nameof(BrokeredMessage));

      var brokeredMessageBodyStream = brokeredMessage?.GetBody<Stream>();
      var body = ReadFully(brokeredMessageBodyStream);

      // ReSharper disable once PossibleNullReferenceException
      var transportMessage = new TransportMessage(brokeredMessage.Properties.ToDictionary(x => x.Key, x => x.Value?.ToString()), body);

      await OnMessageReceive.Invoke(brokeredMessage.LockToken, transportMessage);
    }
  }
}
