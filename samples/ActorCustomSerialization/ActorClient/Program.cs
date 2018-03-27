using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Remoting.V2.FabricTransport.Client;
using MyActor.Interfaces;


namespace ActorClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var actorId = new ActorId(Guid.NewGuid());

            
            var data = new MyData() {Data = "TestData"};

            var proxyFactory = new ActorProxyFactory((c) => new FabricTransportActorRemotingClientFactory(
                fabricTransportRemotingSettings: null,
                serializationProvider: new CustomDataContractProvider()));

            var proxy = proxyFactory.CreateActorProxy<IMyActor>(actorId, "fabric:/ActorCustomDataContractSerialization");

            proxy.SetDataAsync(data, CancellationToken.None).GetAwaiter().GetResult();

            var result = proxy.GetDataAsync(CancellationToken.None).GetAwaiter().GetResult();

            Console.WriteLine(result.Data);
        }
    }
}


