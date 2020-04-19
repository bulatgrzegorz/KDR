using System;

namespace KDR.Abstractions.Messages
{
  public interface ICommand : IMessage
  {
    Guid CorrelationId { get; set; }
  }
}
