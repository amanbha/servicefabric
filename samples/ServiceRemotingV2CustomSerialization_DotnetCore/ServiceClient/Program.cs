using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using Stateful1.Common;
using System;
using System.Threading;

namespace ServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxyFactory = new ServiceProxyFactory((c) => new FabricTransportServiceRemotingClientFactory(
                    serializationProvider: new CustomDataContractProvider()));

            var partitionKey = new ServicePartitionKey(0);
            var proxy = proxyFactory.CreateServiceProxy<IMyService>(new Uri("fabric:/ServiceRemotingV2DotnetCore/Stateful1"), partitionKey);

            var data = new MyData() { Data = "TestData" };
            proxy.SetDataAsync(data, CancellationToken.None).GetAwaiter().GetResult();
            var result = proxy.GetDataAsync(CancellationToken.None).GetAwaiter().GetResult();
        }
    }
}
