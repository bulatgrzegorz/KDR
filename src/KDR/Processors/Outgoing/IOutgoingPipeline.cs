using System.Collections.Generic;

namespace KDR.Processors.Outgoing
{
    public interface IOutgoingPipeline
    {
        ICollection<IOutgoingPipeAction> Actions { get; }
        //Command
        //Performance
        // tracing
        // delayed
        // ticket
        // dtl

        // event
        // Performance
        // tracing
        // delayed
        // dtl
    }
}
