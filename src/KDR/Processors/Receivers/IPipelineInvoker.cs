using System.Threading.Tasks;
using KDR.Transport;

namespace KDR.Processors.Receivers
{
  public interface IPipelineInvoker
  {
    Task InvokeAsync(TransportMessage message);

    Task InvokeAsync(ReceivePipelineContext context);
  }
}
