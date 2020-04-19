using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using KDR.Abstractions.Handlers;
using KDR.Messages;
using KDR.Transport;
using KDR.Transport.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace KDR.Processors.Receivers
{
  public class ReceiversManager
  {
    private readonly ITransportReceiverClientFactory _transportReceiverClientFactory;
    private readonly ICollection<string> _entities;

    private readonly ICollection<ITransportReceiverClient> _receiverClients;
    private readonly IDictionary<string, Type> _handlers;
    private readonly IServiceProvider _serviceProvider;

    public ReceiversManager(ICollection<string> entities, ITransportReceiverClientFactory transportReceiverClientFactory, IServiceProvider serviceProvider)
    {
      _entities = entities;
      _transportReceiverClientFactory = transportReceiverClientFactory;
      _serviceProvider = serviceProvider;
      _receiverClients = new List<ITransportReceiverClient>();
    }

    public async Task Start(CancellationToken cancellationToken)
    {
      foreach (var entity in _entities)
      {
        var receiver = _transportReceiverClientFactory.Create(entity);

        _receiverClients.Add(receiver);

        receiver.OnMessageReceive = ReceiverOnOnMessageReceived;
        await receiver.StartListeningAsync(cancellationToken);
      }
    }

    private Task ReceiverOnOnMessageReceived(object sender, TransportMessage transportMessage)
    {
      var handlerType = _handlers[transportMessage.Headers[MessageHeaders.EventType]];

      var messageHandler = _serviceProvider.GetRequiredService(handlerType) as IMessageHandler;
      if (messageHandler == null)
      {
        throw new NotSupportedException();
      }

      messageHandler.HandleAsync(tra)
    }
  }
}
