using KDR.Messages;

namespace KDR.Processors.Receivers
{
  public interface IReceivePipeContext
  {
    Message Message { get; set; }
  }
}
