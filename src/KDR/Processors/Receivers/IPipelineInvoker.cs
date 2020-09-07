using System.Threading.Tasks;
using KDR.Processors.Outgoing;
using KDR.Transport;
using KDR.Transport.Api;

namespace KDR.Processors.Receivers
{
    public interface IPipelineInvoker
    {
        Task InvokeAsync(TransportMessage message);

        Task InvokeAsync(ReceivePipelineContext context);

        Task InvokeAsync(OutgoingPipelineContext context);
    }
}