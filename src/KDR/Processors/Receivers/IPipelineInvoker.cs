using System.Threading.Tasks;

namespace KDR.Processors.Receivers
{
  public interface IPipelineInvoker
  {
    Task InvokeAsync(IReceivePipeline receivePipeline);
  }
}
