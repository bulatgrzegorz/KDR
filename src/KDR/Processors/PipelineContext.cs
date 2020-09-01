using System.Collections.Concurrent;

namespace KDR.Processors
{
  public abstract class PipelineContext
  {
    private readonly ConcurrentDictionary<string, object> _items = new ConcurrentDictionary<string, object>();

    public T Save<T>(T instance)
    {
      return Save(typeof(T).FullName, instance);
    }

    public T Load<T>()
    {
      return Load<T>(typeof(T).FullName);
    }

    public T Save<T>(string key, T instance)
    {
      _items.AddOrUpdate(key, instance, (s, o) => instance);
      return instance;
    }

    public T Load<T>(string key)
    {
      return _items.TryGetValue(key, out var instance)
               ? (T)instance
               : default;
    }
  }
}
