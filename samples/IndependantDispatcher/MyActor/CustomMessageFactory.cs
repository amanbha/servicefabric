using Microsoft.ServiceFabric.Services.Remoting.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyActor
{
    class CustomMessageFactory : IServiceRemotingMessageBodyFactory
    {
        public IServiceRemotingRequestMessageBody CreateRequest(string interfaceName, string methodName, int numberOfParameters)
        {
            return new ActorRequestMessage(numberOfParameters);
        }

        public IServiceRemotingResponseMessageBody CreateResponse(string interfaceName, string methodName)
        {
            return new ActorResponseMessage();
        }
    }

    class ActorRequestMessage : IServiceRemotingRequestMessageBody
    {
        private Dictionary<string, object> parameters;

        public ActorRequestMessage(int numberOfParameters)
        {
            this.parameters = new Dictionary<string, object>();
        }

        public void SetParameter(int position, string parameName, object parameter)
        {
            this.parameters.Add(parameName, parameter);
        }

        public object GetParameter(int position, string parameName, Type paramType)
        {
            object val;
            this.parameters.TryGetValue(parameName, out val);
            return val;
        }
    }


    class ActorResponseMessage : IServiceRemotingResponseMessageBody
    {
        private object val;

        public void Set(object response)
        {
            this.val = response;
        }

        public object Get(Type paramType)
        {
            return this.val;
        }
    }
}
