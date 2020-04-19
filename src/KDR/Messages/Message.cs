using System.Collections.Generic;

namespace KDR.Messages
{
  public class Message
  {
    public object Body { get; set; }

    public IDictionary<string, string> Headers { get; set; }
  }
}
