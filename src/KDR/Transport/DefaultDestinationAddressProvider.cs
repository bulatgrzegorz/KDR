using System;
using System.Collections.Generic;
using KDR.Transport.Api;

namespace KDR.Transport
{
    public class DefaultDestinationAddressProvider : IDestinationAddressProvider
    {
        private readonly IDictionary<Type, string> _messagesDestinations;

        public DefaultDestinationAddressProvider(IDictionary<Type, string> messagesDestinations = null)
        {
            _messagesDestinations = messagesDestinations ?? new Dictionary<Type, string>();
        }

        public void Add<TMessage>(string destination)
        {
            _messagesDestinations.Add(typeof(TMessage), destination);
        }

        public string Get<TMessage>()
        {
            return _messagesDestinations[typeof(TMessage)];
        }

        public string Get(Type messageType)
        {
            return _messagesDestinations[messageType];
        }
    }
}