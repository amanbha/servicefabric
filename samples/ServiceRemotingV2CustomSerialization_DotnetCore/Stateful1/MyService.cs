using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Stateful1.Common;

namespace Stateful1
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class MyService : StatefulService, IMyService
    {
        private const string StateDictionaryName = "StateDictionary";
        private const long key = 12345;

        public MyService(StatefulServiceContext context)
            : base(context)
        {             
        }

        public async Task<IMyData> GetDataAsync(CancellationToken cancellationToken)
        {
            var stateDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<long, string>>(StateDictionaryName);
            using (var tx = this.StateManager.CreateTransaction())
            {
                var result = await stateDictionary.TryGetValueAsync(tx, key);
                await tx.CommitAsync();
                return new MyData() { Data = result.Value };
            }
        }
        

        public async Task SetDataAsync(IMyData data, CancellationToken cancellationToken)
        {
            var stateDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<long, string>>(StateDictionaryName);
            using (var tx = this.StateManager.CreateTransaction())
            {
                await stateDictionary.AddOrUpdateAsync(tx, key, data.Data, (k, v) => data.Data);
                await tx.CommitAsync();
            }
        }


        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener((context) =>
                {
                    return new FabricTransportServiceRemotingListener(context, this, serializationProvider: new CustomDataContractProvider());
                })
            };
        }        
    }
}
