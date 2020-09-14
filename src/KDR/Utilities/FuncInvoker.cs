using System;
using System.Threading.Tasks;
namespace KDR.Utilities
{
    public static class FuncInvoker
    {
        public static Task Invoke(Func<Task> func) 
        {
            return func?.Invoke() ?? Task.CompletedTask;
        }
    }
}