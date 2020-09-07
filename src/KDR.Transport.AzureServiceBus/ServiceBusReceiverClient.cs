using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus.Management;

namespace KDR.Transport.AzureServiceBus
{
    public class ServiceBusReceiverClient : ITransportReceiverClient
    {
        private readonly MessageReceiver _messageReceiver;

        public async Task sub(string cs)
        {
            var nm = new ManagementClient(cs);
            //var nm = NamespaceManager.CreateFromConnectionString(cs);
            var td = await nm.CreateTopicAsync("KDR.Topic.Test.1");
            var sd = await nm.CreateSubscriptionAsync(td.Path, "KRD.Sub");

            var sb = new SubscriptionClient(cs, td.Path, sd.SubscriptionName, ReceiveMode.PeekLock, RetryPolicy.Default);

            //var messagingFactory = await MessagingFactory.CreateAsync("connectionString");

            //var s = SubscriptionClient.CreateFromConnectionString("cs", "topic", "subscriptionName", ReceiveMode.PeekLock);
            //s.AddRule("", new TrueFilter() { Parameters = { } });
            //var sc = new SubscriptionClient(messagingFactory, "topic", "subscriptionName", ReceiveMode.PeekLock);

            //var messageSender = await messagingFactory.CreateMessageSenderAsync("entityPath");
            //var messageReceiver = await messagingFactory.CreateMessageReceiverAsync("entityPath", ReceiveMode.PeekLock);

            //messageSender.SendAsync(new BrokeredMessage(MemoryStream.in "dead"))
            Message m = null;
            sb.RegisterMessageHandler(
                (message, token) =>
                {
                    m = message;
                    return Task.CompletedTask;
                },
                new MessageHandlerOptions(args => Task.CompletedTask)
                {
                    AutoComplete = false,
                        MaxConcurrentCalls = 1
                });

            //path to receive:
            //for topic: topic/subscriptions/applicationName
            //for command: queue

            //new BrokeredMessage()
            //messageSender.SendAsync()
            //_messageReceiver.
        }
    }
}