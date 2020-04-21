using System;
using System.Threading.Tasks;

namespace KDR.Processors.Receivers
{
  public interface IReceivePipeAction : IPipeAction
  {
    Task ExecuteAsync(ReceivePipeActionContext ctx, Func<Task> next);
  }
}
