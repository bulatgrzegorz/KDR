using System.Collections.Generic;

namespace KDR.Processors.Receivers
{
    public interface IReceivePipeline
    {
        ICollection<IReceivePipeAction> Actions { get; }
    }
}
