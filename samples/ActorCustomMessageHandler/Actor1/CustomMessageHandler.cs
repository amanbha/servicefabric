using Actor1.Interfaces;
using Microsoft.ServiceFabric.Actors.Remoting.V2.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2;
using Microsoft.ServiceFabric.Services.Remoting.V2.Runtime;
using System.Threading.Tasks;

namespace Actor1
{
    public class CustomMessageHandler : IServiceRemotingMessageHandler
    {
        IServiceRemotingMessageHandler innerHandler;

        public CustomMessageHandler(Actor1Service service)
        {
            this.innerHandler = new ActorServiceRemotingDispatcher(service, null);
        }

        public IServiceRemotingMessageBodyFactory GetRemotingMessageBodyFactory()
        {
            return this.innerHandler.GetRemotingMessageBodyFactory();
        }

        public void HandleOneWayMessage(IServiceRemotingRequestMessage requestMessage)
        {
            this.innerHandler.HandleOneWayMessage(requestMessage);
        }

        public Task<IServiceRemotingResponseMessage> HandleRequestResponseAsync(IServiceRemotingRequestContext requestContext, IServiceRemotingRequestMessage requestMessage)
        {
            // Information from Headers can be extracted here from request.
            ThreadCultureInfo.AddCultureInfoToCallContext(requestMessage);
            var response = this.innerHandler.HandleRequestResponseAsync(requestContext, requestMessage);            
            return response;
        }
    }
}
