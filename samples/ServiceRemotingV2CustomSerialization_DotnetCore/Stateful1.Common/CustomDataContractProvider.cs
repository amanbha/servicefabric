using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting.V2;


namespace Stateful1.Common
{
    public class CustomDataContractProvider : IServiceRemotingMessageSerializationProvider
    {
        private readonly ServiceRemotingDataContractSerializationProvider provider;
        private readonly IEnumerable<Type> myTypes;

        public CustomDataContractProvider()
        {
            this.provider = new ServiceRemotingDataContractSerializationProvider();
            this.myTypes = new List<Type>()
            {
                typeof(MyData)
            };
        }
        public IServiceRemotingMessageBodyFactory CreateMessageBodyFactory()
        {
            return this.provider.CreateMessageBodyFactory();
        }

        public IServiceRemotingRequestMessageBodySerializer CreateRequestMessageSerializer(Type serviceInterfaceType,
            IEnumerable<Type> requestBodyTypes)
        {
            var result = requestBodyTypes.Concat(this.myTypes);
            return this.provider.CreateRequestMessageSerializer(serviceInterfaceType, result);
        }

        public IServiceRemotingResponseMessageBodySerializer CreateResponseMessageSerializer(Type serviceInterfaceType,
            IEnumerable<Type> responseBodyTypes)
        {
            var result = responseBodyTypes.Concat(this.myTypes);
            return this.provider.CreateResponseMessageSerializer(serviceInterfaceType, result);
        }
    }
}
