using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.V2;
using Microsoft.ServiceFabric.Actors.Remoting.V2.Runtime;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyActor.Interfaces;

namespace MyActor
{
    public class MyActorService : ActorService, IMyService
    {
        private IServiceRemotingMessageBodyFactory messageBodyFactory;
        private ActorServiceRemotingDispatcher actorServiceRemotingMessageDispatcher;
        private StatefulServiceContext serviceContext;
        private long lowKey;
        private long highkey;

        public MyActorService(StatefulServiceContext context, ActorTypeInformation actorTypeInfo, Func<ActorService, ActorId, ActorBase> actorFactory = null, Func<ActorBase, IActorStateProvider, IActorStateManager> stateManagerFactory = null, IActorStateProvider stateProvider = null, ActorServiceSettings settings = null) : base(context, actorTypeInfo, actorFactory, stateManagerFactory, stateProvider, settings)
        {
            this.messageBodyFactory = new CustomMessageFactory();
            this.serviceContext = context;
            var fabricClient = new FabricClient();

            var partitionInfo = (Int64RangePartitionInformation)fabricClient.QueryManager.GetPartitionAsync(this.Context.PartitionId).GetAwaiter().GetResult()[0].PartitionInformation;
            this.lowKey = partitionInfo.LowKey;
            this.highkey = partitionInfo.HighKey;           
        }

        protected override Task OnOpenAsync(ReplicaOpenMode openMode, CancellationToken cancellationToken)
        {
            this.actorServiceRemotingMessageDispatcher = new ActorServiceRemotingDispatcher(this, this.messageBodyFactory);
            return Task.FromResult(true);
        }

        protected override Task RunAsync(CancellationToken cancellationToken)
        {
            return base.RunAsync(cancellationToken);

            // You can start a timer to get data data from external queue and invoke Actors OR push to this ActorService from some other external entity by making call to SignalToProcess using Serviceproxy.
        }

        public async Task<string> DispatchToActor(ActorId actorId, string actorInterfaceName, string actorMethodName, string dataToProcess)
        {
            // Create headers
            var header = new ActorRemotingDispatchHeaders();
            header.ActorId = actorId;
            header.MethodName = actorMethodName;
            header.ActorInterfaceName = actorInterfaceName;

            // Add params
            var body = this.messageBodyFactory.CreateRequest(actorInterfaceName, actorMethodName, 1);
            body.SetParameter(0, "dataToProcess", dataToProcess);

            var response = await this.actorServiceRemotingMessageDispatcher.HandleRequestResponseAsync(header, body, CancellationToken.None);            

            return (string)response.Get(typeof(string));
        }

        private IEnumerable<ActorId> GetActorsFromExternalQueue()
        {
            var actors = new List<ActorId>();

            // Get Actors Information from your external queue.

            // add some test data.
            var actorId = new ActorId(this.lowKey);
            actors.Add(actorId);

            return actors;
        }

        public Task SignalToProcess()
        {
            this.StartProcessing();
            return Task.CompletedTask;
        }

        public void StartProcessing()
        {
            foreach (var actorId in this.GetActorsFromExternalQueue())
            {
                // dispatch only if the actor id belongs to current partition.
                if (DoesActorBelongToThisPartition(actorId))
                {
                    var result = this.DispatchToActor(actorId, typeof(IMyActor).FullName, "Process", "TestData").GetAwaiter().GetResult();
                }
            }
        }

        private bool DoesActorBelongToThisPartition(ActorId actorId)
        {
            var partitionKey = actorId.GetPartitionKey();
            return partitionKey >= this.lowKey && partitionKey <= this.highkey;
        }
    }
}
