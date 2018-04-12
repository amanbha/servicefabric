using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2;
using Microsoft.ServiceFabric.Services.Remoting.V2.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.Runtime;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceClient
{
    class CustomServiceRemotingClientFactory : IServiceRemotingClientFactory
    {
        private IServiceRemotingClientFactory innerFactory;

        public CustomServiceRemotingClientFactory()
        {
            this.innerFactory = new FabricTransportServiceRemotingClientFactory();
            this.innerFactory.ClientConnected += this.ClientConnected;
            this.innerFactory.ClientDisconnected += this.ClientDisconnected;

        }

        public event EventHandler<CommunicationClientEventArgs<IServiceRemotingClient>> ClientConnected;
        public event EventHandler<CommunicationClientEventArgs<IServiceRemotingClient>> ClientDisconnected;

        public async Task<IServiceRemotingClient> GetClientAsync(Uri serviceUri, ServicePartitionKey partitionKey, TargetReplicaSelector targetReplicaSelector, string listenerName, OperationRetrySettings retrySettings, CancellationToken cancellationToken)
        {
            var remotingClient = await this.innerFactory.GetClientAsync(
                serviceUri,
                partitionKey,
                targetReplicaSelector,
                listenerName,
                retrySettings,
                cancellationToken);

            return new CustomServiceRemotingClient(remotingClient);
        }

        public async Task<IServiceRemotingClient> GetClientAsync(ResolvedServicePartition previousRsp, TargetReplicaSelector targetReplicaSelector, string listenerName, OperationRetrySettings retrySettings, CancellationToken cancellationToken)
        {
            var remotingClient = await this.innerFactory.GetClientAsync(
                previousRsp,
                targetReplicaSelector,
                listenerName,
                retrySettings,
                cancellationToken);

            return new CustomServiceRemotingClient(remotingClient);
        }

        public IServiceRemotingMessageBodyFactory GetRemotingMessageBodyFactory()
        {
            return this.GetRemotingMessageBodyFactory();
        }

        public Task<OperationRetryControl> ReportOperationExceptionAsync(IServiceRemotingClient client, ExceptionInformation exceptionInformation, OperationRetrySettings retrySettings, CancellationToken cancellationToken)
        {
            return this.ReportOperationExceptionAsync(
                (client as CustomServiceRemotingClient).InnerClient,
                exceptionInformation,
                retrySettings,
                cancellationToken);
        }
    }
}
