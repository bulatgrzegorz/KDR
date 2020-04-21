using System.Collections.Generic;

namespace KDR.Processors.Receivers
{
  public class ReceivePipeline : IReceivePipeline
  {
    public ReceivePipeline(ICollection<IReceivePipeAction> actions = null)
    {
      Actions = actions ?? new List<IReceivePipeAction>();
    }

    public ICollection<IReceivePipeAction> Actions { get; }

    public ReceivePipeline AddAction(IReceivePipeAction action)
    {
      Actions.Add(action);
    }
  }
}
