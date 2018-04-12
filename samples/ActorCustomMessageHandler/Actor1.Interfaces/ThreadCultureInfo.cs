using Microsoft.ServiceFabric.Services.Remoting.V2;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace Actor1.Interfaces
{
    public class ThreadCultureInfo
    {
        private const string KeyName = "__CultureInfo__";

        public static void AddCultureInfoToHeader(IServiceRemotingRequestMessage requestMessage, string contextData)
        {
            requestMessage.GetHeader().AddHeader(KeyName, Encoding.UTF8.GetBytes(contextData));
        }

        public static void AddCultureInfoToCallContext(IServiceRemotingRequestMessage requestMessage)
        {
            byte[] headerValue;
            string contextData = null;
            
            if (requestMessage.GetHeader().TryGetHeaderValue(KeyName, out headerValue))
            {
                contextData = Encoding.UTF8.GetString(headerValue);
            }

            CallContext.LogicalSetData(KeyName, contextData);
        }

        public static bool TryGetCultureInfoFromCallContext(out string contextData)
        {
            contextData = (string)CallContext.LogicalGetData(KeyName);

            return (contextData != null);
        }
    }
}
