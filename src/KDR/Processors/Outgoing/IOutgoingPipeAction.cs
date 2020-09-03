using System;
using System.Threading.Tasks;

namespace KDR.Processors.Outgoing
{
    public interface IOutgoingPipeAction : IPipeAction<OutgoingPipelineContext>
    {
        new Task ExecuteAsync(OutgoingPipelineContext ctx, Func<Task> next);
    }
}