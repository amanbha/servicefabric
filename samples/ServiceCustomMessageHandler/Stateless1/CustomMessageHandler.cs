using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.V2;
using Microsoft.ServiceFabric.Services.Remoting.V2.Runtime;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stateless1
{
    public class CustomMessageHandler : IServiceRemotingMessageHandler
    {
        IServiceRemotingMessageHandler innerHandler;

        public CustomMessageHandler(ServiceContext serviceContext, IService serviceImplementation)
        {
            this.innerHandler = new ServiceRemotingMessageDispatcher(serviceContext, serviceImplementation);
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
            // Custom events can be hooked here.
            // Information from Headers can be extracted here.
            var messageHeaders = requestMessage.GetHeader();
            var interfaceId = messageHeaders.InterfaceId;
            var methodId = messageHeaders.MethodId;
            CustomRemotingEvents.RaiseReceiveRequest(string.Empty);
            var response = this.innerHandler.HandleRequestResponseAsync(requestContext, requestMessage);
            CustomRemotingEvents.RaiseSendResponse(string.Empty);
            return response;
        }
    }
}
