using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KDR.Transport.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace KRD.TestClient1
{
  internal class Program
  {
    public static async Task<TResult> ReadJson<TResult>(byte[] body)
    {
      //var serializer = new JsonSerializer { TypeNameHandling = TypeNameHandling.Auto };
      return JsonConvert.DeserializeObject<TResult>(Encoding.UTF8.GetString(body));
      //using (var reader = new StreamReader(stream))
      //{
      //  var s = reader.ReadToEnd();
      //  return JsonConvert.DeserializeObject<TResult>(s);
      //}
      //using (var jsonReader = new JsonTextReader(reader))
      //{
      //  jsonReader.ReadAsString()
      //  jsonReader.SupportMultipleContent = false;

      //  var st = await jsonReader.ReadAsStringAsync();
      //  return JsonConvert.DeserializeObject<TResult>(st);
      //}
    }

    private static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");

      var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json");

      var config = builder.Build();
      var a = config.GetConnectionString("SB");

      var s = new ServiceBusReceiverClient(new ServiceBusTransportOptions(a, "kdr.topic.test.1/subscriptions/KDR.Sub", 1));

      //var t = new TestEvent("ts", 12, new List<(int a, string b)>() { (1, "a"), (2, "b") });
      //var se = JsonConvert.SerializeObject(t);
      //var ms = new MemoryStream(Encoding.UTF8.GetBytes(se));

      // s.OnMessageReceived += (sender, message) =>
      //                        {
      //                          Console.WriteLine(ReadJson<TestEvent>(message.Body));

      //                          s.CommitAsync(sender, CancellationToken.None).GetAwaiter().GetResult();
      //                        };
      s.StartListeningAsync(CancellationToken.None).GetAwaiter().GetResult();

      while (true)
      {
        if (Console.ReadKey().Key == ConsoleKey.S)
        {
          break;
        }
      }
    }

    public class TestEvent
    {
      public TestEvent(string testString, int testInt, ICollection<(int a, string b)> testCollection)
      {
        TestString = testString;
        TestInt = testInt;
        TestCollection = testCollection;
      }

      public string TestString { get; }

      public int TestInt { get; }

      public ICollection<(int a, string b)> TestCollection { get; }
    }
  }
}
