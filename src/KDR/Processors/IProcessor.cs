using System.Threading.Tasks;

namespace KDR.Processors
{
    public interface IProcessor
    {
        Task<bool> ProcessAsync(ProcessingContext context);
    }
}