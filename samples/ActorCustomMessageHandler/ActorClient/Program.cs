using Actor1.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using System.Threading;

namespace ActorClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxyFactory = new ActorProxyFactory((c) =>
            {
                return new CustomActorRemotingClientFactory(c);
            });

            var actorId = new ActorId("test");
            var proxy = proxyFactory.CreateActorProxy<IActor1>(actorId, "fabric:/ActorCustomMessageHandler");
            var res = proxy.GetContextData(CancellationToken.None).GetAwaiter().GetResult();
        }
    }
}
