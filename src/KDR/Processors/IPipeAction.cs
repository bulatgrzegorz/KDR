using System;
using System.Threading.Tasks;

namespace KDR.Processors
{
  public interface IPipeAction<TPipeContext>
  {
    Task ExecuteAsync(TPipeContext ctx, Func<Task> next);
  }
}
