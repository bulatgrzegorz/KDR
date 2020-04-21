using System.Threading.Tasks;
using KDR.Abstractions.Messages;

namespace KDR.Abstractions.Handlers
{
  //Trzeba jakoś przekazywać headery, albo użyć klasy bazowej z kontekstem - albo jakiegoś asyncLocal, callContext w scope, albo coś z DI?
  public interface IMessageHandler
  {
    Task HandleAsync(IMessage message);
  }
}
