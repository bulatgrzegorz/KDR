using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KDR.Abstractions.Handlers;
using KDR.Abstractions.Messages;
using KDR.Messages;
using KDR.Serialization;
using KDR.Transport;
using KDR.Transport.Api;
using KDR.Transport.Api.Factories;

namespace KDR.Processors.Receivers
{
  public class ReceiversManager
  {
    private readonly ITransportReceiverClientFactory _transportReceiverClientFactory;
    private readonly ICollection<string> _entities;
    private readonly ICollection<ITransportReceiverClient> _receiverClients;
    private readonly IPipelineInvoker _pipelineInvoker;

    public ReceiversManager(
      ICollection<string> entities,
      ITransportReceiverClientFactory transportReceiverClientFactory,
      IPipelineInvoker pipelineInvoker)
    {
        _entities = entities;
        _transportReceiverClientFactory = transportReceiverClientFactory;
        _receiverClients = new List<ITransportReceiverClient>();
        _pipelineInvoker = pipelineInvoker;
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

    private async Task ReceiverOnOnMessageReceived(object sender, TransportMessage transportMessage, Func<object, Task> commitAction)
    {
      var ctx = new ReceivePipelineContext();
      ctx.Save<TransportMessage>(transportMessage);
      ctx.Save<Func<Task>>(ReceivePipelineContext.CommitMessageAction, () => commitAction(sender));

      await _pipelineInvoker.InvokeAsync(ctx);
    }
  }
}
