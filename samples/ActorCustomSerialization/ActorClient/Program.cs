using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Query;
using Microsoft.ServiceFabric.Actors.Remoting.V2.FabricTransport.Client;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using MyActor.Interfaces;


namespace ActorClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxyFactory = new ActorProxyFactory((c) => new FabricTransportActorRemotingClientFactory(
                    fabricTransportRemotingSettings: null,
                    serializationProvider: new CustomDataContractProvider()));

            for (int i = 0; i < 200; i++)
            {
                var actorId = new ActorId(Guid.NewGuid());
                var data = new MyData() { Data = "TestData" };
                var proxy = proxyFactory.CreateActorProxy<IMyActor>(actorId, "fabric:/ActorCustomDataContractSerialization");
                proxy.SetDataAsync(data, CancellationToken.None).GetAwaiter().GetResult();
                var result = proxy.GetDataAsync(CancellationToken.None).GetAwaiter().GetResult();
            }

            // Query
            QueryAtors();
        }

        private static void QueryAtors()
        {   
            var fabricClient = new FabricClient();
            var serviceUri = new Uri("fabric:/ActorCustomDataContractSerialization/MyActorService");
            var partitionList = fabricClient.QueryManager.GetPartitionListAsync(serviceUri).GetAwaiter().GetResult();

            foreach (var partition in partitionList)
            {
                ContinuationToken continuationToken = null;
                var queriedActorCount = 0;
                var queriedActiveActorCount = 0;
                var queriedInactiveActorCount = 0;


                var proxyFactory = new ServiceProxyFactory((c) =>
                {
                    return new FabricTransportServiceRemotingClientFactory(
                        serializationProvider: new CustomDataContractProvider());
                });

                do
                {
                    var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                    var actorServiceProxy = proxyFactory.CreateServiceProxy<IActorService>(serviceUri, partitionKey);
                    var queryResult = actorServiceProxy.GetActorsAsync(continuationToken, CancellationToken.None)
                        .GetAwaiter().GetResult();
                    queriedActorCount += queryResult.Items.Count();
                    queriedActiveActorCount += queryResult.Items.Count(actor => actor.IsActive);
                    queriedInactiveActorCount += queryResult.Items.Count(actor => !actor.IsActive);
                    continuationToken = queryResult.ContinuationToken;
                } while (continuationToken != null);


                Console.WriteLine($"Partition Id: {partition.PartitionInformation.Id},  Total: {queriedActorCount}, Active: {queriedActiveActorCount}, Inactive {queriedInactiveActorCount}");

            }
        }
    }
}


