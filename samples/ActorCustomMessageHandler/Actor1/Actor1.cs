using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Actor1.Interfaces;

namespace Actor1
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.None)]
    internal class Actor1 : Actor, IActor1
    {
        /// <summary>
        /// Initializes a new instance of Actor1
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public Actor1(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        public Task<string> GetContextData(CancellationToken cancellationToken)
        {
            ThreadCultureInfo.TryGetCultureInfoFromCallContext(out var contextData);
            return Task.FromResult(contextData);
        }        
    }
}
