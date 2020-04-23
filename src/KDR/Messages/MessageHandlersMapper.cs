using System;
using System.Collections.Generic;

namespace KDR.Messages
{
    public static class MessageHandlersMapper
    {
        private static readonly IDictionary<string, Type> _messageHandlersType;

        static MessageHandlersMapper()
        {
            _messageHandlersType = new Dictionary<string, Type>();
        }

        public static void AddMapping(string messageType, Type handlerType)
        {
            _messageHandlersType[messageType] = handlerType;
        }

        public static Type GetHandler(string messageType)
        {
            if(!_messageHandlersType.TryGetValue(messageType, out var handlerType))
            {
                throw new NotSupportedException();
            }

            return handlerType;
        }
    }
}