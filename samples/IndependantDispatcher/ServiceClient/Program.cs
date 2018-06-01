using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using MyActor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FabricTransportServiceRemotingProvider(RemotingListener = RemotingListener.V2Listener, RemotingClient = RemotingClient.V2Client)]
namespace ServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxyFactory = new ServiceProxyFactory((c) =>
            {
                return new FabricTransportServiceRemotingClientFactory();
            });

            var serviceProxy = proxyFactory.CreateServiceProxy<IMyService>(new Uri("fabric:/IndependantDispatcher/MyActorService"), new ServicePartitionKey(0));
            serviceProxy.SignalToProcess().GetAwaiter().GetResult();
        }
    }
}
