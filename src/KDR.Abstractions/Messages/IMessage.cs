using System;

namespace KDR.Abstractions.Messages
{
    public interface IMessage 
    { 
        Guid CorrelationId { get; }
    }
}