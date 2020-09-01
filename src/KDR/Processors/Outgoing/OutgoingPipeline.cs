using System.Collections.Generic;

namespace KDR.Processors.Outgoing
{
    public class OutgoingPipeline : IOutgoingPipeline
    {
        public ICollection<IOutgoingPipeAction> Actions { get; }

        public OutgoingPipeline(ICollection<IOutgoingPipeAction> actions = null)
        {
            Actions = actions ?? new List<IOutgoingPipeAction>();
        }

        public OutgoingPipeline AddAction(IOutgoingPipeAction action)
        {
            Actions.Add(action);

            return this;
        }
    }
}