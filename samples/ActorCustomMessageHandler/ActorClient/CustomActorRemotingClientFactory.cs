using Microsoft.ServiceFabric.Actors.Remoting.V2.FabricTransport.Client;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting.V2;
using Microsoft.ServiceFabric.Services.Remoting.V2.Client;
using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;

namespace ActorClient
{
    class CustomActorRemotingClientFactory : IServiceRemotingClientFactory
    {
        private IServiceRemotingClientFactory innerFactory;

        public CustomActorRemotingClientFactory(IServiceRemotingCallbackMessageHandler handler)
        {
            this.innerFactory = new FabricTransportActorRemotingClientFactory(
                new FabricTransportRemotingSettings(),
                handler);
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

            return new CustomActorRemotingClient(remotingClient);
        }

        public async Task<IServiceRemotingClient> GetClientAsync(ResolvedServicePartition previousRsp, TargetReplicaSelector targetReplicaSelector, string listenerName, OperationRetrySettings retrySettings, CancellationToken cancellationToken)
        {
            var remotingClient = await this.innerFactory.GetClientAsync(
                previousRsp,
                targetReplicaSelector,
                listenerName,
                retrySettings,
                cancellationToken);

            return new CustomActorRemotingClient(remotingClient);
        }

        public IServiceRemotingMessageBodyFactory GetRemotingMessageBodyFactory()
        {
            return this.innerFactory.GetRemotingMessageBodyFactory();
        }

        public Task<OperationRetryControl> ReportOperationExceptionAsync(IServiceRemotingClient client, ExceptionInformation exceptionInformation, OperationRetrySettings retrySettings, CancellationToken cancellationToken)
        {
            return this.ReportOperationExceptionAsync(
                (client as CustomActorRemotingClient).InnerClient,
                exceptionInformation,
                retrySettings,
                cancellationToken);
        }
    }
}
