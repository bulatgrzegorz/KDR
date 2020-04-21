using System;

namespace KDR.Persistence
{
  public class DbMessage
  {
    public int Id { get; set; }

    public string Content { get; set; }

    public DateTime Created { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public int Retries { get; set; }
  }
}
