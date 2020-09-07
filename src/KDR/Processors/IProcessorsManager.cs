using System.Threading.Tasks;

namespace KDR.Processors
{
    public interface IProcessorsManager
    {
        Task StartAsync();

        Task DisposeAsync();
    }
}