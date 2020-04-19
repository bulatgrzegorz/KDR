using System.Text;
using Newtonsoft.Json;

namespace KDR.Serialization
{
  public class JsonSerializer
  {
    private static readonly Encoding DefaultEncoding = Encoding.UTF8;

    private static readonly JsonSerializerSettings DefaultSettings = new JsonSerializerSettings()
    { TypeNameHandling = TypeNameHandling.All };
  }
}
