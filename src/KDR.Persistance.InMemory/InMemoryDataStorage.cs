using System.Collections.Concurrent;
using System.Threading.Tasks;
using KDR.Messages;

namespace KDR.Persistence.InMemory
{
  public class InMemoryDataStorage : IDataStorage
  {
    private readonly ConcurrentBag<DbMessage> _messagesToSent;

    private readonly ConcurrentBag<Message> _receivedMessages;

    public InMemoryDataStorage()
    {
      _messagesToSent = new ConcurrentBag<DbMessage>();
      _receivedMessages = new ConcurrentBag<Message>();
    }

    public Task StoreMessageToSendAsync(DbMessage message)
    {
      _messagesToSent.Add(message);

      return Task.CompletedTask;
    }

    public Task<bool> StoreReceivedMessageAsync(Message message)
    {
      _receivedMessages.Add(message);

      return Task.FromResult(result: true);
    }
  }
}
