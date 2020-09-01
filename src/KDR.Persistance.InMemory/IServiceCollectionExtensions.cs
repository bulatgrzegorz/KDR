using KDR.Persistence.Api;
using Microsoft.Extensions.DependencyInjection;

namespace KDR.Persistence.InMemory
{
  public static class IServiceCollectionExtensions
  {
    public static void AddServices(this IServiceCollection services)
    {
      services.AddSingleton<IDataStorage, InMemoryDataStorage>();
    }
  }
}
