using System;
namespace KDR.Transport.Api
{
    public interface IDestinationAddressProvider
    {
         string Get<TMessage>();

         string Get(Type messageType);
    }
}