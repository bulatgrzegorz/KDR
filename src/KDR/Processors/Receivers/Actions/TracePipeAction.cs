using KDR.Processors.Actions;
using Microsoft.Extensions.Logging;

namespace KDR.Processors.Receivers.Actions
{
    public class TracePipeAction : TracePipeActionBase<ReceivePipelineContext>, IReceivePipeAction 
    { 
        public TracePipeAction(ILogger<TracePipeAction> logger) : base(logger)
        {
            
        }
    }
}