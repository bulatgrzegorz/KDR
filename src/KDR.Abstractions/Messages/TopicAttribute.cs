using System;

namespace KDR.Abstractions.Messages
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  public abstract class TopicAttribute : Attribute
  {
    protected TopicAttribute(string name)
    {
      Name = name;
    }

    public string Name { get; }
  }
}
