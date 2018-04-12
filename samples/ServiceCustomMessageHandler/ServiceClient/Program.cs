using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using Stateless1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var proxy = proxyFactory.CreateServiceProxy<IMyService>(new Uri("fabric:/ServiceCustomMessageHandler/Stateless1"));
            var res = proxy.ComputeSum(2, 3).GetAwaiter().GetResult();
        }
    }
}
