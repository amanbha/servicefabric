using Microsoft.ServiceFabric.Services.Remoting.V2;
using Microsoft.ServiceFabric.Services.Remoting.V2.Client;
using Stateless1;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceClient
{
    public class CustomServiceRemotingClient : IServiceRemotingClient
    {
        public IServiceRemotingClient InnerClient { get; }

        public CustomServiceRemotingClient(IServiceRemotingClient remotingClient)
        {
            this.InnerClient = remotingClient;
        }


        public ResolvedServicePartition ResolvedServicePartition
        {
            get { return this.InnerClient.ResolvedServicePartition; }
            set { this.InnerClient.ResolvedServicePartition = value; }
        }
        
        
        public string ListenerName
        {
            get { return this.InnerClient.ListenerName; }
            set { this.InnerClient.ListenerName = value; }
        }
        

        public ResolvedServiceEndpoint Endpoint
        {
            get { return this.InnerClient.Endpoint; }
            set { this.InnerClient.Endpoint = value; }
        }

        public Task<IServiceRemotingResponseMessage> RequestResponseAsync(IServiceRemotingRequestMessage requestMessage)
        {
            // Add context data.
            CultureInfo.AddContextDataToHeader(requestMessage, Thread.CurrentThread.CurrentCulture.Name);
            return this.InnerClient.RequestResponseAsync(requestMessage);
        }

        public void SendOneWay(IServiceRemotingRequestMessage requestMessage)
        {
            this.InnerClient.SendOneWay(requestMessage);
        }
    }
}
