using KDR.Processors.Actions;
using Microsoft.Extensions.Logging;

namespace KDR.Processors.Outgoing.Actions
{
    public class TracePipeAction : TracePipeActionBase<OutgoingPipelineContext>, IOutgoingPipeAction
    {
        public TracePipeAction(ILogger<TracePipeAction> logger) : base(logger)
        {
            
        }
    }
}