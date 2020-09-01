using System.Collections.Generic;

namespace KDR.Persistence
{
  public class ReceivedDbMessage
  {
    public object Body { get; set; }

    public IDictionary<string, string> Headers { get; set; }
  }
}