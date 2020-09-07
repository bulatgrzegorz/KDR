using System;
using System.Threading.Tasks;

namespace KDR.Processors.Receivers
{
    public interface IReceivePipeAction : IPipeAction<ReceivePipelineContext>
    {
        new Task ExecuteAsync(ReceivePipelineContext ctx, Func<Task> next);
    }
}